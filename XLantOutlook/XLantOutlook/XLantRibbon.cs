using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Tools.Ribbon;
using Outlook = Microsoft.Office.Interop.Outlook;
using XLantCore;
using XLForms;
using System.Windows.Forms;
using System.IO;


namespace XLantOutlook
{
    public partial class XLantRibbon
    {
        public static List<XLantCore.XLMain.Client> recentClients = new List<XLMain.Client>();
        public static List<XLMain.EntityCouplet> staff = new List<XLMain.EntityCouplet>();
        public static Boolean exInternal = false;
        private static Outlook.Items newItems;

        private void XLantRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            //Deal with the prompt issue
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\personal.xml";
            XDocument xPer = XLtools.personalDoc();
            XElement settings = xPer.Descendants("PersonalSettings").FirstOrDefault();
            string prompt = "";
            if (settings.Descendants("Prompt").FirstOrDefault() == null)
            {
                settings.Add(new XElement("Prompt", "true"));
                xPer.Save(docPath);
            }
            else
            {
                prompt = settings.Descendants("Prompt").FirstOrDefault().Value;
            }

            //check the setting for prompt if it is not present or true then add handler
            if (prompt != "false")
            {
            XLOutlook.XLEventhandler(true);
            PromptTglBtn.Checked = true;
            }
            else
            {
                //don't need to stop the event handler, it hasn't yet been started
                PromptTglBtn.Checked = false;
            }
            
            if (prompt == "true")
            {
                //make the selection visible
                ExInternalCheck.Visible = true;
                //handle whether internal emails should be excluded
                if (settings.Descendants("Internal").FirstOrDefault() == null)
                {
                    settings.Add(new XElement("Internal", "false"));
                    xPer.Save(docPath);
                }

                if (settings.Descendants("Internal").FirstOrDefault().Value == "true")
                {
                    ExInternalCheck.Checked = true;
                    exInternal = true;
                }
                else
                {
                    ExInternal.Checked = false;
                    exInternal = false;
                }
            }
            else
            {
                ExInternal.Visible = false;
            }
            //Handle the creation of the VC Folder
            //XLOutlook.CreateVCFolder();
        }

        private void IndexBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = XLOutlook.GetSelectedEmail();
            Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            string path = "";
            path = explorer.CurrentFolder.FolderPath;

            XLOutlook.QuickIndex(email, path);

            //see whether there are some new connections to be made
            string emails = XLOutlook.EmailAddressesStr(email);

            explorer.CurrentFolder.CurrentView.Apply();
        }

        private void PromptTglBtn_Click(object sender, RibbonControlEventArgs e)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\personal.xml";
            //Update the settings file
            XDocument xPer = XLtools.personalDoc();
            XElement settings = xPer.Descendants("PersonalSettings").FirstOrDefault();
            string prompt = settings.Descendants("Prompt").FirstOrDefault().Value;
            if (PromptTglBtn.Checked)
            {
                //Update the settings file
                settings.Descendants("Prompt").FirstOrDefault().Value = "true";
                xPer.Save(docPath);
                XLOutlook.XLEventhandler(true);
                ExInternalCheck.Visible = true;
            }
            else
            {
                //Update the settings file
                settings.Descendants("Prompt").FirstOrDefault().Value = "false";
                xPer.Save(docPath);
                XLOutlook.XLEventhandler(false);
                ExInternalCheck.Visible = false;
            }
        }

        private void QuickIndex_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = XLOutlook.GetSelectedEmail();
            Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            string path = "";
            path = explorer.CurrentFolder.FolderPath;

            XLOutlook.QuickIndex(email, path);
            explorer.CurrentFolder.CurrentView.Apply();
        }

        private void MultiClientIndexBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = XLOutlook.GetSelectedEmail();
            Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            string path = "";
            path = explorer.CurrentFolder.FolderPath;

            XLOutlook.MultiQuickIndex(email, path);
            explorer.CurrentFolder.CurrentView.Apply();
        }

        private void indexAttachmentsBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = XLOutlook.GetSelectedEmail();
            XLOutlook.IndexAttachments(email);
        }

        private void ExInternal_Click(object sender, RibbonControlEventArgs e)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\XLant\\personal.xml";
            //Update the settings file
            XDocument xPer = XLtools.personalDoc();
            XElement settings = xPer.Descendants("PersonalSettings").FirstOrDefault();
            if (ExInternal.Checked)
            {
                //Update the settings file
                settings.Descendants("Internal").FirstOrDefault().Value = "true";
                xPer.Save(docPath);
                exInternal = true;
            }
            else
            {
                //Update the settings file
                settings.Descendants("Internal").FirstOrDefault().Value = "false";
                xPer.Save(docPath);
                exInternal = false;
            }
        }

        //private void Test_Click(object sender, RibbonControlEventArgs e)
        //{
        //    XLTask.CheckToDos();
        //}

        //private void AssignBtn_Click(object sender, RibbonControlEventArgs e)
        //{
        //    //get the selected task
        //    Outlook.TaskItem task = XLOutlook.GetSelectedTask();

        //    //get the VCid of the file
        //    string fileId = XLOutlook.ReadParameter(XLTask.fileIdName, task);
        //    MessageBox.Show(fileId);

        //    //and then find the client it relates to
        //    XLMain.Client client = XLVirtualCabinet.GetClientFromIndex(fileId);

        //    //from there collect the users list and launch the dialog
        //    XLMain.Staff writer = XLMain.Staff.StaffFromUser(Environment.UserName);
        //    if (XLantRibbon.staff.Count == 0)
        //    {
        //        XLantRibbon.staff = XLMain.Staff.AllStaff();
        //    }
        //    ForwardForm myForm = new ForwardForm(client, writer, XLantRibbon.staff);
        //    myForm.ShowDialog();
        //    XLMain.EntityCouplet staff = myForm.selectedStaff;

        //    //create the command file
        //    string commandfileloc = "";
        //    commandfileloc = XLVirtualCabinet.Reindex(fileId, staff.name);

        //    //Launch the reindex
        //    XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true);
        //    if (result.ExitCode != 0)
        //    {
        //        MessageBox.Show("Reindex failed please complete manually.");
        //    }
        //    else
        //    {
        //        //mark the task as completed now that it is someone elses problem
        //        task.Complete = true;
        //    }
        //}

        //private void ApproveBtn_Click(object sender, RibbonControlEventArgs e)
        //{
        //    //get the selected task
        //    Outlook.TaskItem task = XLOutlook.GetSelectedTask();

        //    //get the VCid of the file
        //    string fileId = XLOutlook.ReadParameter(XLTask.fileIdName, task);
        //    MessageBox.Show(fileId);

        //    //and then find the client it relates to
        //    XLMain.Client client = XLVirtualCabinet.GetClientFromIndex(fileId);

        //    //from there collect the users list and launch the dialog
        //    XLMain.Staff writer = XLMain.Staff.StaffFromUser(Environment.UserName);
        //    if (XLantRibbon.staff.Count == 0)
        //    {
        //        XLantRibbon.staff = XLMain.Staff.AllStaff();
        //    }
        //    ForwardForm myForm = new ForwardForm(client, writer, XLantRibbon.staff);
        //    myForm.ShowDialog();
        //    XLMain.EntityCouplet staff = myForm.selectedStaff;

        //    //create the command file
        //    string commandfileloc = "";
        //    commandfileloc = XLVirtualCabinet.Reindex(fileId, staff.name, "Approved");

        //    //Launch the reindex
        //    XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true);
        //    if (result.ExitCode != 0)
        //    {
        //        MessageBox.Show("Reindex failed please complete manually.");
        //    }
        //    else
        //    {
        //        //mark the task as completed now that it is someone elses problem
        //        task.Complete = true;
        //    }
        //}


        //private void ExtractBtn_Click(object sender, RibbonControlEventArgs e)
        //{
        //    Outlook.TaskItem task = XLOutlook.GetSelectedTask();

        //    //get the VCid of the file
        //    string fileId = XLOutlook.ReadParameter(XLTask.fileIdName, task);

        //    if (fileId != null)
        //    {

        //        //get the file info
        //        XLVirtualCabinet.FileInfo info = XLVirtualCabinet.FileIndex(fileId);

        //        //create a commandfile to download the file
        //        string folderpath = XLtools.TempPath();
        //        string commandfilepath = "";
        //        commandfilepath = folderpath + (String.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now)) + ".bond";
        //        StreamWriter commandfile = new StreamWriter(commandfilepath, false, System.Text.Encoding.Default);

        //        commandfile.WriteLine("<<MODE=EXTRACT>>");
        //        commandfile.WriteLine("<<LOCATION=" + folderpath + ">>");
        //        commandfile.WriteLine("<<EXTRACTFILENAME=INDEX01>>");
        //        commandfile.WriteLine("<<INDEX01=" + fileId + ">>");
        //        commandfile.Flush();
        //        commandfile.Close();

        //        // Call Bond to check out the document
        //        XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfilepath, false);

        //        // Dispose of the command file object
        //        commandfile.Dispose();

        //        //attach the file we just extracted
        //        string fileLocation = folderpath + fileId + info.Extension;
        //        task.Attachments.Add(fileLocation);
        //        task.Save();

        //        //once saved tot he task delete it
        //        File.Delete(fileLocation);
        //    }
        //}

        //private void button1_Click(object sender, RibbonControlEventArgs e)
        //{
        //    Outlook.TaskItem task = XLOutlook.GetSelectedTask();
        //    string fileId = XLOutlook.ReadParameter(XLTask.fileIdName, task);
        //    MessageBox.Show(fileId);
        //}

    }
}