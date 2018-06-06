using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using XLant;
using XLForms;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace XLantOutlook
{

    public static class XLOutlook
    {
        private static Outlook.Items sentItems = null;
        //private static XLMain.Staff user = null;
        //private static string toBeIndex = null;
        //private static string descIndex = null;
        private static List<string> emailsToIndex = new List<string>();
        //private static System.Threading.Timer timer;

        public static XLant.XLVirtualCabinet.BondResult IndexEmail(Outlook.MailItem email, string folder)
        {
            try
            {
                
                XLMain.Client selectClient = new XLMain.Client();
                
                selectClient = GetClient(email);
            
                //Add CRMID to email for later use.
                UpdateParameter("CrmID", selectClient.crmID, email);
                //Collect data for indexing
                XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);
            
                string docPath = TempSave(email);
                string docDate = DateTime.Now.ToString("dd/MM/yyyy");
                //take a guess at the status
                string status = "";
                string desc = email.Subject;
                if (folder.Contains("Inbox"))
                {
                    status = "External";
                    docDate = CheckDate(email.ReceivedTime);
                }
                else if (folder.Contains("Sent"))
                {
                    status = "Sent";
                    docDate = CheckDate(email.SentOn); 
                }
                else
                {
                    status = "Draft";
                    docDate = CheckDate(DateTime.Now);
                }
                if (XLantRibbon.staff.Count == 0)
                {
                    XLantRibbon.staff = XLMain.Staff.AllStaff();
                }

                VCForm indexForm = new VCForm(user, selectClient, docPath, desc, status, docDate, "blank", XLantRibbon.staff);
                indexForm.ShowDialog();
                //collect result from form
                XLVirtualCabinet.BondResult outcome = indexForm.outcome;

                //add client to recent list
                AddClienttoRecent(selectClient);

                return outcome;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to index e-mail");
                XLtools.LogException("IndexEmail", ex.ToString());
                return null;
            }
         }

        public static XLMain.Client GetClient(Outlook.MailItem email)
        {
            XLMain.Client selectClient = new XLMain.Client();
            string strippedemails = EmailAddressesStr(email).ToString();
            string strippedSubject = "";
            if (email.Subject != null)
            {
                strippedSubject = email.Subject;
                string[] embargoWords = { "Milsted Langdon", "Limited", "PLC", "Mr", "Mrs", "Miss", "the", "and", "A", "of", "Partnership", "LLP", "Liquidation", "Administration", "Bankruptcy", "individual", "voluntary arrangement", "accounts", "audit", "tax" };
                foreach (string word in embargoWords)
                {
                    strippedSubject = strippedSubject.Replace(word, "");
                }
            }

            string query = "SELECT clientcrmid as crmid, display as name FROM [XLant].[dbo].[FuzzyClientSearch] (@param1,@param2) order by score desc";

            ClientForm myForm = new ClientForm(query, strippedemails, strippedSubject, XLantRibbon.recentClients);
            myForm.ShowDialog();
            selectClient = myForm.selectedClient;

            return selectClient;
        }

        public static string[] EmailAddresses(Outlook.MailItem email, bool incML = false)
        {
            //find the client from e-mail
            //collect all the email addressess.
            string emails = email.SenderEmailAddress;

            const string PR_SMTP_ADDRESS = "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
            foreach (Outlook.Recipient rec in email.Recipients)
            {
                Outlook.PropertyAccessor pa = rec.PropertyAccessor;
                string s = pa.GetProperty(PR_SMTP_ADDRESS).ToString();
                s = s.ToLower();
                if (!incML)
                {
                    if (!s.Contains("milsted-langdon"))
                    {
                        emails += ";" + pa.GetProperty(PR_SMTP_ADDRESS).ToString();
                    }
                }
            }
            //Remove any unwanted '
            emails = emails.Replace("'", "");
            string[] addresses = emails.Split(';');

            return addresses;
        }

        public static string EmailAddressesStr(Outlook.MailItem email, bool incML = false)
        {
            //find the client from e-mail
            //collect all the email addressess.
            string emails = email.SenderEmailAddress;

            const string PR_SMTP_ADDRESS = "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
            foreach (Outlook.Recipient rec in email.Recipients)
            {
                Outlook.PropertyAccessor pa = rec.PropertyAccessor;
                string s = pa.GetProperty(PR_SMTP_ADDRESS).ToString();
                s = s.ToLower();
                if (!incML)
                {
                    if (!s.Contains("milsted-langdon"))
                    {
                        emails += ";" + pa.GetProperty(PR_SMTP_ADDRESS).ToString();
                    }
                }
            }
            //Remove any unwanted '
            emails = emails.Replace("'", "");
            return emails;
        }

        public static string TempSave(Outlook.MailItem email)
        {
            try
            {
                //blanked in an attempt to handle the weird random saving
                string folder = "";
                folder = XLtools.TempPath();
                Random rnd = new Random();
                string filename = "";

                if (email.Subject == null)
                {
                    filename = "Email";
                }
                else
                {
                    filename = email.Subject;
                }
            
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    filename = filename.Replace(c.ToString(), "");
                }

                int id = rnd.Next(1000, 9999); //provides a four digit id
                filename += "-" + id.ToString();
                filename += ".msg";
                //If that file already exists try again until it doesn't
                while (File.Exists(folder + filename))
                {
                    filename = DateTime.Now.ToString("yyyy-MM-dd");
                    id = rnd.Next(1000, 9999); //provides a four digit id
                    filename += "-" + id.ToString();
                    filename += ".msg";
                }
                filename = folder + filename;
                email.SaveAs(filename);

                return filename;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to save e-mail");
                XLtools.LogException("Outlook-TempSave", ex.ToString());
                return null;
            }
        }

        public static void UpdateParameter(string pName, string pValue, Outlook.MailItem email)
        {
            try
            {
                Outlook.ItemProperties properties = email.ItemProperties; 
                if (properties.Cast<Outlook.ItemProperty>().Where(c => c.Name == pName).Count() == 0)
                {
                    properties.Add(pName, Outlook.OlUserPropertyType.olText);
                }
                properties[pName].Value = pValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update parameter");
                XLtools.LogException("Outlook-UpdateParameter", ex.ToString());
            }
        }

        public static void UpdateParameter(string pName, string pValue, Outlook.TaskItem task)
        {
            try
            {
                Outlook.ItemProperties properties = task.ItemProperties;
                if (properties.Cast<Outlook.ItemProperty>().Where(c => c.Name == pName).Count() == 0)
                {
                    properties.Add(pName, Outlook.OlUserPropertyType.olText);
                }
                properties[pName].Value = pValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update parameter");
                XLtools.LogException("Outlook-UpdateParameter", ex.ToString());
            }
        }

        public static void UpdateVCTick(Outlook.MailItem email)
        {
            try
            {
                Outlook.ItemProperties properties = email.ItemProperties;
                if (properties.Cast<Outlook.ItemProperty>().Where(c => c.Name == "In Virtual Cabinet").Count() == 0)
                {
                    properties.Add("In Virtual Cabinet", Outlook.OlUserPropertyType.olYesNo);
                }
                properties["In Virtual Cabinet"].Value = 1;

                //Change category
                //check there isn't already a category assigned
                if (email.Categories == null)
                {
                    //discover the VC category and assign it
                    string catName = OutlookCategory();
                    if (catName != null)
                    {
                        var vCCat = catName;
                        email.Categories = vCCat;
                    }
                }
                email.Save();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Unable to update parameter");
                XLtools.LogException("Outlook-UpdateParameter", ex.ToString());
            }
        }

        public static string ReadParameter(string pName, Outlook.MailItem email)
        {
            try
            {
                Outlook.ItemProperties properties = email.ItemProperties; 
                foreach (Outlook.ItemProperty prop in properties)
                {
                    if (prop.Name == pName)
                    {
                        return prop.Value.ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to read parameter");
                XLtools.LogException("Outlook-ReadParameter", ex.ToString());
                return null; 
            }
        }

        public static string ReadParameter(string pName, Outlook.TaskItem task)
        {
            try
            {
                Outlook.ItemProperties properties = task.ItemProperties;
                foreach (Outlook.ItemProperty prop in properties)
                {
                    if (prop.Name == pName)
                    {
                        return prop.Value.ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to read parameter");
                XLtools.LogException("Outlook-ReadParameter", ex.ToString());
                return null;
            }
        }

        public static Outlook.MailItem GetSelectedEmail()
        {
            try
            {
                Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                if (explorer.Selection.Count == 1)
                {
                    Object selItem = explorer.Selection[1];
                    if (selItem is Outlook.MailItem)
                    {
                        Outlook.MailItem email = (Outlook.MailItem)selItem;
                        return email;
                    }
                    else
                    {
                        MessageBox.Show("This facility currently only handles email items");
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("This facility will not handle bulk uploads");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to select e-mail");
                XLtools.LogException("GetSelectedEmail", ex.ToString());
                return null;
            }
        }

        public static Outlook.TaskItem GetSelectedTask()
        {
            try
            {
                Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
                if (explorer.Selection.Count == 1)
                {
                    Object selItem = explorer.Selection[1];
                    if (selItem is Outlook.TaskItem)
                    {
                        Outlook.TaskItem task = (Outlook.TaskItem)selItem;
                        return task;
                    }
                    else
                    {
                        MessageBox.Show("This facility currently only handles tasks items");
                        return null;
                    }
                }
                else
                {
                    MessageBox.Show("This facility will not handle bulk selection");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to select e-mail");
                XLtools.LogException("GetSelectedEmail", ex.ToString());
                return null;
            }
        }

        public static string OutlookCategory()
        {
            try
            {
                //try and find the outlook settings file for VC
                string outlookCategory="";
                XDocument settingsDoc = XDocument.Load(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Lindenhouse Software Ltd\\Virtual Cabinet Office Addin\\Outlook\\Config\\configuration.xml");
                string set = "false";
                
                //pull out the appropriate section
                XElement settings = (from options in settingsDoc.Descendants("FlaggingGeneralOpts")
                                select options).FirstOrDefault();
                if (settings != null)
                {
                    //cycle through the settings to find the two that we need.
                    foreach (XElement setting in settings.Descendants())
                    {
                        if (setting.Name == "SetCategory")
                        {
                            set = setting.Value;
                        }
                        if (setting.Name == "CategoryName")
                        {
                            outlookCategory = setting.Value;
                        }
                    }
                }
                //if set category is true then return category else return null
                if (set == "true")
                {
                    return outlookCategory;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to ascertain category");
                XLtools.LogException("OutlookCategory", ex.ToString());
                return null;
            }
        }

        public static string FileLocation(Outlook.MailItem email)
        {
            try
            {
                string loc = "";
                string[] sArray = email.Headers("Message-ID");
                //Should only ever be 1 message-ID!
                string id = "";
                if (sArray.Length == 1)
                {
                    id = sArray[0];
                    MessageBox.Show(id);
                }
                else
                {
                    MessageBox.Show("Unable to find message Id");
                }

                DataTable fileid = XLSQL.ReturnTable("Select fileid + '-' + version from VCMailID where MailID = '" + id + "'");
                string filename = "";
                if (fileid != null)
                {
                    filename = fileid.Rows[0][0].ToString();
                    MessageBox.Show(filename);
                }
                else
                {
                    MessageBox.Show("Unable to find file ID");
                }

                string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Virtual Cabinet\\Edited documents\\";

                loc = folder + filename;
                return loc;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to get file location");
                XLtools.LogException("FileLocation", ex.ToString());
                return null;
            }
        }

        public static void QuickIndex(Outlook.MailItem email, string folder)
        {
            try
            {
                XLant.XLVirtualCabinet.BondResult outcome = IndexEmail(email, folder);

                if (outcome.ExitCode != 0)
                {
                    MessageBox.Show("Unable to index document, please index manually.  Error code: " + outcome.ExitCode.ToString() + "-" + outcome.StandardOutput.ToString());
                }
                else
                {
                    //update the tick box and category
                    UpdateVCTick(email);
                    //delete email from temp directory
                    if (File.Exists(outcome.DocPath))
                    {
                        File.Delete(outcome.DocPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to index email");
                XLtools.LogException("QuickIndex", ex.ToString());
            }
        }

        public static void MultiQuickIndex(Outlook.MailItem email, string folder)
        {
            try
            {
                XLant.XLVirtualCabinet.BondResult outcome = IndexEmail(email, folder);
                // As the filing has been successfull, get the FileId returned from Bond via the Standard Output
                if (outcome.ExitCode == 0)
                {
                    string fileid = Regex.Match(outcome.StandardOutput, @"\d+").ToString();
                    XLVirtualCabinet.FileInfo info = XLVirtualCabinet.FileIndex(fileid);
                    DialogResult result = MessageBox.Show("Do you want to index another copy", "Index", MessageBoxButtons.YesNo);
                    while (result == DialogResult.Yes)
                    {
                        XLForms.ClientForm myForm = new ClientForm();
                        myForm.ShowDialog();
                        XLMain.Client client = myForm.selectedClient;
                        //update the cabinet based on the new client
                        string cabinet = XLVirtualCabinet.FileStore(client.manager.office, client.department);
                        info.Cabinet = cabinet;
                        //update the client field
                        foreach (XLVirtualCabinet.IndexPair pair in info.Indexes)
                        {
                            if (pair.index == "INDEX02")
                            {
                                pair.value = client.clientcode + " - " + client.name;
                            }
                        }
                        outcome = XLVirtualCabinet.IndexDocument(outcome.DocPath, info);

                        if (outcome.ExitCode == 0)
                        {
                            result = MessageBox.Show("Do you want to index another copy", "Index", MessageBoxButtons.YesNo);
                            continue;
                        }
                        else
                        {
                            MessageBox.Show("Unable to index document, please index manually.  Error code: " + outcome.ExitCode.ToString() + "-" + outcome.StandardOutput.ToString());
                            break;
                        }
                    }
                    //update the tick box and category
                    UpdateVCTick(email);
                    //delete email from temp directory
                    if (File.Exists(outcome.DocPath))
                    {
                        File.Delete(outcome.DocPath);
                    }
                }
                else
                {
                    Exception e = new Exception("Unable to index");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to index email");
                XLtools.LogException("MultiQuickIndex", ex.ToString());
            }
        }

        public static void IndexDraft(Outlook.MailItem email)
        {
            try
            {

                XLant.XLVirtualCabinet.BondResult outcome = IndexEmail(email, "Draft");

                if (outcome.ExitCode != 0)
                {
                    MessageBox.Show("Unable to index document, please index manually.  Error code: " + outcome.ExitCode.ToString() + "-" + outcome.StandardOutput.ToString());
                }
                else
                {
                    UpdateVCTick(email);
                    // As the filing has been successfull, get the FileId returned from Bond via the Standard Output
                    string fileid = Regex.Match(outcome.StandardOutput, @"\d+").ToString();
                    string folderpath = XLtools.TempPath();
                    string commandfilepath = "";
                    commandfilepath = folderpath + "\\" + (String.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now)) + ".bond";
                    StreamWriter commandfile = new StreamWriter(commandfilepath, false, System.Text.Encoding.Default);

                    commandfile.WriteLine("<<MODE=EDIT>>");
                    commandfile.WriteLine("<<INDEX01=" + fileid + ">>");
                    commandfile.WriteLine("<<OPENDOCUMENT=FALSE>>");
                    commandfile.Flush();
                    commandfile.Close();

                    // Call Bond to check out the document
                    XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfilepath, false);
                    // Dispose of the command file object
                    commandfile.Dispose();

                    // Look for the file in the edited documents folder based on the FileId
                    List<string> msgfile = new List<string>();
                    string[] Files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Virtual Cabinet\\Edited documents");
                    int FilesCount = Files.Length;

                    // Collect all the entries which match the fileID
                    for (int i = 0; i < FilesCount; i++)
                    {
                        string f = Files[i].ToString();

                        if (Path.GetFileName(f).StartsWith(fileid + "-"))
                            msgfile.Add(f);
                    }

                    if (msgfile.Count != 1)
                    {
                        Exception exception = new Exception("Unable to find the mail message.  Your file has been indexed but could not have the file ID added.");
                    }
                    else
                    {

                        // Add the Virtual Cabinet FileId to the Subject of the email (still open on screen)
                        // UserProperties and Custom Headers do not persist, so using something that does. Body could be another option.
                        XLOutlook.UpdateParameter("VCFileID", fileid, email);
                        //email.Subject = email.Subject + @" FileId:" + fileid.ToString();

                        // Save the MailItem
                        email.Save();

                        // Save the email in the default MSG format
                        if (File.Exists(msgfile[0]))
                        {
                            File.Delete(msgfile[0]);
                        }
                        email.SaveAs(msgfile[0]);

                        // Create a command file to save the document as a new version based on the FileId
                        commandfilepath = folderpath + "\\" + (String.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now)) + ".bond";
                        commandfile = new StreamWriter(commandfilepath, false, System.Text.Encoding.Default);
                        commandfile.WriteLine("<<MODE=SAVE>>");
                        commandfile.WriteLine("<<INDEX01=" + fileid + ">>");
                        commandfile.Flush();
                        commandfile.Close();

                        // Call Bond to save the email back to VC
                        result = XLVirtualCabinet.LaunchCabi(commandfilepath, false);
                        // Dispose of the command file object
                        commandfile.Dispose();

                        // Close the email in Outlook to prevent further changes that won't be saved to VC
                        email.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olSave);

                        // Delete the email from the Drafts folder in Outlook
                        email.UnRead = false;
                        email.Delete();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to index draft email");
                XLtools.LogException("IndexDraft", ex.ToString());
            }
        }

        public static void OverwriteDraft(Outlook.MailItem email)
        {
            try
            {
                //get the fileID from the stored Parameter
                string fileId = XLOutlook.ReadParameter("VCFileID", email);

                //reindex the email to remove the to be actioned by and update status and doc date
                string commandfileloc = "";
                commandfileloc = XLVirtualCabinet.Reindex(fileId, status: "Sent", docDate: email.SentOn.ToString("dd/MM/yyyy"));

                XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true);
                
                if (result.ExitCode != 0)
                {
                    MessageBox.Show("Reindex failed please complete manually.");
                }
                else
                {
                    //If reindex successful then continue
                    //create a commandfile to reopen the email in edit mode
                    string folderpath = XLtools.TempPath();
                    string commandfilepath = "";
                    commandfilepath = folderpath + "\\" + (String.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now)) + ".bond";
                    StreamWriter commandfile = new StreamWriter(commandfilepath, false, System.Text.Encoding.Default);

                    commandfile.WriteLine("<<MODE=EDIT>>");
                    commandfile.WriteLine("<<INDEX01=" + fileId + ">>");
                    commandfile.WriteLine("<<OPENDOCUMENT=FALSE>>");
                    commandfile.Flush();
                    commandfile.Close();

                    // Call Bond to check out the document
                    result = XLVirtualCabinet.LaunchCabi(commandfilepath, false);
                    // Dispose of the command file object
                    commandfile.Dispose();

                    // Look for the file in the edited documents folder based on the FileId
                    List<string> msgfile = new List<string>();
                    string[] Files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Virtual Cabinet\\Edited documents");
                    int FilesCount = Files.Length;

                    // Collect all the entries which match the fileID
                    for (int i = 0; i < FilesCount; i++)
                    {
                        string f = Files[i].ToString();

                        if (Path.GetFileName(f).StartsWith(fileId + "-"))
                            msgfile.Add(f);
                    }
                    //There should only be 1 if there are more something has gone wrong. We don't want to overwrite the wrong one so exit
                    if (msgfile.Count != 1)
                    {
                        Exception exception = new Exception("Unable to find the draft email message.");
                    }
                    else
                    {

                        // Delete the old version and save the sent email in the default MSG format
                        if (File.Exists(msgfile[0]))
                        {
                            File.Delete(msgfile[0]);
                        }
                        email.SaveAs(msgfile[0]);

                        // Create a command file to save the document as a new version based on the FileId
                        commandfilepath = folderpath + "\\" + (String.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now)) + ".bond";
                        commandfile = new StreamWriter(commandfilepath, false, System.Text.Encoding.Default);
                        commandfile.WriteLine("<<MODE=SAVE>>");
                        commandfile.WriteLine("<<INDEX01=" + fileId + ">>");
                        commandfile.Flush();
                        commandfile.Close();

                        // Call Bond to save the email back to VC
                        result = XLVirtualCabinet.LaunchCabi(commandfilepath, false);
                        // Dispose of the command file object
                        commandfile.Dispose();
                        //update the tick box
                        UpdateVCTick(email);
                        //Mark email as saved.
                        email.UnRead = false;
                        //Close and save
                        email.Close(Outlook.OlInspectorClose.olSave);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update e-mail");
                XLtools.LogException("OverwriteDraft", ex.ToString());
            }
        }

        public static void XLEventhandler(bool status)
        {
            try
            {

                Outlook.Application app = Globals.ThisAddIn.Application;
                Outlook.MAPIFolder sentfolder = app.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderSentMail);


                sentItems = sentfolder.Items;
                if (status)
                {
                    //True turns it on
                    sentItems.ItemAdd += new Outlook.ItemsEvents_ItemAddEventHandler(SentItemIndexer);
                }
                else
                {
                    //false turns it off
                    sentItems.ItemAdd -= new Outlook.ItemsEvents_ItemAddEventHandler(SentItemIndexer);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to setup handler");
                XLtools.LogException("XLEventHanlder", ex.ToString());
            }
        }

        //strip out the VC property on receipt so that replies and forwards report correctly
        //redundant as the VC addin will only add it back
        public static void ItemSent(object item)
        {
            try
            {
                Outlook.MailItem email = (Outlook.MailItem)item;
                Outlook.ItemProperties properties = email.ItemProperties;
                Outlook.ItemProperty property = properties.Cast<Outlook.ItemProperty>().Where(c => c.Name == "In Virtual Cabinet").FirstOrDefault();
                string fileId = XLOutlook.ReadParameter("VCFileID", email);
                //Only if the e-mail is not a draft handled within VC
                //strip out the field
                if (property != null && (fileId == null || fileId == ""))
                {
                    MessageBox.Show(property.Name + ":" + property.Value);
                    property.Delete();
                    MessageBox.Show("Deleted");
                }
                XLOutlook.UpdateParameter("VCFileID", "", email);
                //for testing purposes try it again to make sure it is gone
                property = properties.Cast<Outlook.ItemProperty>().Where(c => c.Name == "In Virtual Cabinet").FirstOrDefault();
                if (property == null)
                {
                    MessageBox.Show("Gone");
                }
                else
                {
                    MessageBox.Show(property.Name + ":" + property.Value);
                }
            }
            catch (Exception ex)
            {
                XLtools.LogException("ItemSent", ex.ToString());
            }
        }

        ////log the emails which are sent to then pass on to the handler.
        //private static void LogSentEmail(object item)
        //{
        //    try
        //    {
        //        Outlook.MailItem email = (Outlook.MailItem)item;
        //        if (!emailsToIndex.Contains(email.EntryID))
        //        {
        //            emailsToIndex.Add(email.EntryID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        XLtools.LogException("LogEmailSent-Not an email", ex.ToString());
        //    }

        //}

        //////this timer will periodically check the list
        //public async Task PeriodicCheck()
        //{
        //    try
        //    {
        //        //timer = new System.Threading.Timer((e) =>
        //        //{
        //        //    PromptForEmails();
        //        //}, null, 0, 2000);
        //        while (true)
        //        {
        //            await Task.Delay(2000);
        //            PromptForEmails();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Periodic check of e-mails sent has failed.  Please restart Outlook, if this problem persists please contact IT");
        //        XLtools.LogException("PeriodicCheck", ex.ToString());
        //    }

        //}

        //this then reviews the list and if appropriate runs the prompt method on the e-mails listed
        private static void PromptForEmails()
        {
            try
            {
                if (emailsToIndex.Count > 0)
                {
                    //MessageBox.Show(emailsToIndex.ToString());
                    foreach (string s in emailsToIndex.ToList())
                    {
                        Outlook.MailItem item;
                        Outlook.Application app = Globals.ThisAddIn.Application;
                        Outlook.NameSpace nameSpace = app.GetNamespace("MAPI");
                        item = (Outlook.MailItem)nameSpace.GetItemFromID(s);
                        SentItemIndexer(item);
                    }
                }
            }
            catch (Exception ex)
            {
                XLtools.LogException("PromptforEmails", ex.ToString());
            }
        }

        //this manages the process of promping and then indexing the email, if needed

        private static void SentItemIndexer(object item)
        {
            try
            {
                //convert the object to an email type
                Outlook.MailItem email = (Outlook.MailItem)item;
                //get the fileId if there is one
                string fileId = XLOutlook.ReadParameter("VCFileID", email);
                if (fileId != "")
                {
                    //if the fileId exists offer to overwrite the original
                    System.Threading.Thread.Sleep(5000);
                    //check whether the item is a task or not
                    //if (email.IsMarkedAsTask)
                    //{
                    //    //If it is reassign the to do based on the assingee
                    //    XLTask.ReassignTask(email);
                    //}
                    //else
                    //{

                        DialogResult response = MessageBox.Show("Do you want to Overwrite the existing draft email?", "Overwrite?", MessageBoxButtons.OKCancel);
                        if (response == DialogResult.OK)
                        {
                            XLOutlook.OverwriteDraft(email);
                        }
                    //}
                }
                else
                {
                    //otherwise consider indexing
                    //if user wants to exclude internal emails check that first.
                    if (XLantRibbon.exInternal)
                    {
                        //check whether it is internal only
                        string[] addresses = EmailAddresses(email);
                        //it includes the sender so 1 means there are no other external addresses
                        if (addresses.Count() == 1)
                        {
                            //The user wants to ignore internal e-mails and this one has no external parties so nothing 
                            //to do
                            return;
                        }
                    }
                    //check whether conversation reference is turned on
                    if (!email.Body.Contains("VCID:"))
                    {
                        //otherwise continue
                        //get the subject line
                        string str = "";
                        if (email.Subject.Length > 30)
                        {
                            str = email.Subject.Substring(0, 30);
                        }
                        else
                        {
                            str = email.Subject;
                        }
                        //and ask the user whether they want to index
                        DialogResult response = MessageBox.Show("Do you want to index the email: " + str + "...?", "Quick Index?", MessageBoxButtons.OKCancel);
                        if (response == DialogResult.OK)
                        {
                            //if yes carryout indexing.
                            XLOutlook.QuickIndex(email, "Sent");
                        }
                    }
                }
                email.Save();
                emailsToIndex.Remove(email.EntryID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Index Item");
                XLtools.LogException("SentItemHandler", ex.ToString());
            }
        }
        
        public static void IndexAttachments(Outlook.MailItem email)
        {
        	if (email.Attachments.Count > 0)
        	{
        		XLVirtualCabinet.BondResult outcome = new XLVirtualCabinet.BondResult();
                string fileid = "";
                string docPath = "";
                XLVirtualCabinet.FileInfo info = new XLVirtualCabinet.FileInfo();
        		for(int i = 1; i <= email.Attachments.Count; i++)
        		{
        			string folder = XLtools.TempPath();
        			if ( i == 1 )
        			{
        				email.Attachments[i].SaveAsFile(folder + email.Attachments[i].FileName);
        				XLMain.Client selectClient = GetClient(email);
        				//Collect data for indexing
		                XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);
		                docPath = folder + email.Attachments[i].FileName;
                        string docDate = DateTime.Now.ToString("dd/MM/yyyy");
		                string status = "External";
		                string desc = email.Attachments[i].FileName;
                        if (XLantRibbon.staff.Count == 0)
                        {
                            XLantRibbon.staff = XLMain.Staff.AllStaff();
                        }
                        VCForm indexForm = new VCForm(user, selectClient, docPath, desc, status, docDate, "blank", XLantRibbon.staff);
                		indexForm.ShowDialog();
                		//collect result from form
                	 	outcome = indexForm.outcome;

                        //get the details of the file.
                        fileid = Regex.Match(outcome.StandardOutput, @"\d+").ToString();
                        info = XLVirtualCabinet.FileIndex(fileid);

                		//add client to recent list
                		AddClienttoRecent(selectClient);
        			}
        			else
        			{
        				//save the next file
                        email.Attachments[i].SaveAsFile(folder + email.Attachments[i].FileName);
                        docPath = folder + email.Attachments[i].FileName;
		                
                    	foreach (XLVirtualCabinet.IndexPair pair in info.Indexes)
                        {
                            if (pair.index.ToUpper() == "INDEX03")
                            {
                                string d = email.Attachments[i].FileName;
                                //generate form for the description to be altered
                                SingleDataCaptureForm myForm = new SingleDataCaptureForm("Input Description", "Enter Description", d);
                                myForm.ShowDialog();
                                if (myForm.result == DialogResult.Cancel)
                                {
                                    continue;
                                }
                                else
                                {
                                    pair.value = myForm.data;
                                }
                            }
                        }
                        outcome = XLVirtualCabinet.IndexDocument(docPath, info);
        			}
        		}
        		MessageBox.Show("All Attachments Saved");
        	}
        }
        
        private static string CheckDate(DateTime date)
        {
            //Force date format to deal with the issues we are having with document dates

            string str = date.ToString("dd/MM/yyyy");
            return str;

            //DateTime d = new DateTime();

            //System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentUICulture;
            //if (ci.DateTimeFormat.ShortDatePattern == "dd/MM/yyyy")
            //{
            //    return date;
            //}
            //else
            //{
            //    string str = date.ToString("dd/MM/yyyy");
            //    d = DateTime.Parse(str);
            //    return d;
            //}        
        }

        private static void AddClienttoRecent(XLMain.Client client)
        {
            //add client to list
            XLantRibbon.recentClients.Insert(0, client);
            //If the this has created a duplicate delete it
            XLantRibbon.recentClients = XLantRibbon.recentClients.GroupBy(p => p.crmID).Select(g => g.First()).ToList();
            //if now more than ten remove last
            if (XLantRibbon.recentClients.Count>10)
            {
                //remove the 11th item (NB 0 based index)
                XLantRibbon.recentClients.RemoveAt(10);
            }
        }




        //public static void UpdateContacts()
        //{
        //    try
        //    {
        //        //Get current user
        //        XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);
        //        //Get the contacts connected with that user
        //        List<XLMain.Contact> contacts = XLMain.Contact.Contacts(user.crmID);
        //        foreach (XLMain.Contact cont in contacts )
        //        {
        //            if (!CheckContact(cont.crmID))
        //            {
        //                AddContact(cont.crmID);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Unable to update contact");
        //        XLtools.LogException("UpdateContact", ex.ToString());
        //    }
        //}

        //private static Boolean CheckContact(string crmID)
        //{
        //    try
        //    {
        //        Outlook.MAPIFolder folderContacts = Globals.ThisAddIn.Application.ActiveExplorer().Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts); 
        //        Outlook.Items searchFolder = folderContacts.Items;
        //        int counter = 0;
        //        foreach (Outlook.ContactItem foundContact in searchFolder)
        //        {
        //            if (foundContact.User1 == crmID)
        //            {
        //                counter = counter + 1;
        //            }
        //        }
        //        if (counter == 0)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Unable to check the contact with id: " + crmID);
        //        XLtools.LogException("Checkcontact", crmID + " - " + ex.ToString());
        //        return false;
        //    }
        //}

        //private static void AddContact(string crmID)
        //{
        //    Outlook.ContactItem newContact = (Outlook.ContactItem)
        //        Globals.ThisAddIn.Application.CreateItem(Outlook.OlItemType.olContactItem);
        //    try
        //    {
        //        //Get Contact
        //        XLMain.Contact cont = XLMain.Contact.FetchContact(crmID);

        //        newContact.FullName = cont.firstname + " " + cont.lastname;
        //        newContact.CompanyName = cont.organisation.name;
        //        newContact.User1 = cont.crmID;
        //        newContact.PrimaryTelephoneNumber = cont.numbers[0].number;
        //        newContact.MailingAddressStreet = cont.addresses[0].address1;
        //        newContact.MailingAddressCity = cont.addresses[0].address2;
        //        newContact.MailingAddressState = cont.addresses[0].address3;
        //        newContact.MailingAddressPostalCode = cont.addresses[0].address4;
        //        newContact.MailingAddressCountry = cont.addresses[0].address5;
        //        newContact.BusinessFaxNumber = XLMain.Number.GetNumber(cont.crmID, "Fax").number;
        //        newContact.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("The new contact was not saved:" + ex.ToString());
        //        XLtools.LogException("AddContact", ex.ToString());
        //    }
        //}
    }

}
