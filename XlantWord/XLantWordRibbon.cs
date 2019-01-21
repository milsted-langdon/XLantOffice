using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using XLant;
using XLForms;
using System.Xml.Linq;
using Microsoft.Office.Core;

namespace XlantWord
{
    public partial class XLantWordRibbon
    {
        public static XLTaskPane xlTaskPane1;
        public static SigTaskPane sigPane;
        public static Microsoft.Office.Tools.CustomTaskPane CustomxlTaskPane;
        public static List<XLMain.EntityCouplet> userList;
        

        //########################The following declarations and methods are different in .NET 3.5######################//

        private void XLantWordRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Word.View currentView = Globals.ThisAddIn.Application.ActiveWindow.View;

                if (currentView.ShowHiddenText)
                {
                    ShowHiddenBtn.Checked = true;
                }
                else
                {
                    ShowHiddenBtn.Checked = false;
                }
            }
            catch (Exception ex)
            {
                XLtools.LogException("No document loaded", ex.Message);
            }
        }

        //###################After this point all code is identical in both versions############################//

        private void HeaderFooterBtn_Click(object sender, RibbonControlEventArgs e)
        {
            HeaderForm myForm = new HeaderForm();
            myForm.Show();
        }

        private void ShowHiddenBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Microsoft.Office.Interop.Word._Application app = Globals.ThisAddIn.Application;
            Microsoft.Office.Interop.Word.Shape tBox;
            Microsoft.Office.Interop.Word.View currentView = app.ActiveWindow.View;
            
            foreach (Microsoft.Office.Interop.Word.Shape s in app.ActiveDocument.Shapes)
            {
                if(s.Name == "MLStatus")
                {
                    tBox = s;
                    if (tBox.Visible == MsoTriState.msoTrue)
                    {
                        tBox.Visible = MsoTriState.msoFalse;
                        currentView.ShowHiddenText = false;
                    }
                    else
                    {
                        tBox.Visible = MsoTriState.msoTrue;
                        currentView.ShowHiddenText = true;
                    }
                    break;
                }
            }
        }

        private void ApproveBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Word._Application app = Globals.ThisAddIn.Application;

                XLDocument.ChangeStatus("Approved");
                XLMain.Client client = null;
                string fileID = XLDocument.GetFileID();
                if (String.IsNullOrEmpty(fileID))
                {
                    MessageBox.Show("Unable to find FileID.  If the document has not yet been placed in VC click First Index" + Environment.NewLine + "alternatively reindex the document with Virtual Cabinet.");
                    Exception ex = new Exception("Failed to find File ID");
                }

                if (XLDocument.ReadParameter("CRMid") != null)
                {
                    client = XLMain.Client.FetchClient(XLDocument.ReadParameter("CRMid"));
                }
                else
                {
                    //if the document param doesn't exist get the index data from VC
                    client = XLVirtualCabinet.GetClientFromIndex(fileID);
                }
                XLMain.Staff writer = new XLMain.Staff();
                string writerID = XLDocument.ReadParameter("Sender");
                if (writerID == "")
                {
                    writer = XLMain.Staff.StaffFromUser(Environment.UserName);
                }
                else
                {
                    writer = XLMain.Staff.StaffFromUser(XLDocument.ReadParameter("Sender"));
                }
                StaffSelectForm myForm = new StaffSelectForm(client, writer);
                myForm.ShowDialog();
                XLMain.EntityCouplet staff = myForm.selectedStaff;

                if (myForm.DialogResult == DialogResult.OK) 
                {
                    if (staff == null)
                    {
                        //make blank if no staff selected
                        staff.name = "";
                    }
                    string commandfileloc = "";
                    if (XLDocument.ReadBookmark("Date") == "")
                    {
                        commandfileloc = XLVirtualCabinet.Reindex(fileID, staff.name, XLDocument.ReadParameter("VCStatus"));
                    }
                    else
                    {
                        string docDate = XLDocument.ReadBookmark("Date");
                        commandfileloc = XLVirtualCabinet.Reindex(fileID, staff.name, XLDocument.ReadParameter("VCStatus"), docDate);
                    }
                    XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true);
                    if (result.ExitCode != 0)
                    {
                        MessageBox.Show("Reindex failed please complete manually");
                    }
                    else
                    {
                        app.ActiveDocument.Save();
                        app.ActiveWindow.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to approve document");
                XLtools.LogException("ApproveBtn", ex.ToString());
            }
        }

        private void CreateLetterBtn_Click(object sender, RibbonControlEventArgs e)
        {
            LaunchClientDoc("Letter");
        }

        private void CreateFaxBtn_Click(object sender, RibbonControlEventArgs e)
        {
            LaunchClientDoc("Fax");
        }

        private void CreateFNBtn_Click(object sender, RibbonControlEventArgs e)
        {
            LaunchClientDoc("FileNote");
        }

        private void CreateBtns_Click(object sender, RibbonControlEventArgs e)
        {
            LaunchClientDoc("Letter");
        }

        private void PrintBtn_Click(object sender, RibbonControlEventArgs e)
        {
            PrintForm myForm = new PrintForm();
            myForm.Show();
        }

        private void IndexBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                XLDocument.currentDoc.Save();

                //populate description
                string desc = XLDocument.ReadParameter("DocType");
                desc += " to ";
                desc += XLDocument.ReadBookmark("Addressee");
                string str = XLDocument.ReadParameter("CRMid");
                XLMain.Client client = new XLMain.Client();
                if (str != "")
                {
                    client = XLMain.Client.FetchClient(str);
                }
                else
                {
                    ClientForm cForm = new ClientForm();
                    cForm.ShowDialog();
                    client = cForm.selectedClient;
                }
                XLMain.Staff writer = new XLMain.Staff();
                string writerID = XLDocument.ReadParameter("Sender");
                if (writerID == "")
                {
                    writer = XLMain.Staff.StaffFromUser(Environment.UserName);
                }
                else
                {
                    writer = XLMain.Staff.StaffFromUser(XLDocument.ReadParameter("Sender"));
                }

                VCForm myForm = new VCForm(writer, client, XLDocument.currentDoc.FullName, desc, XLDocument.ReadParameter("VCStatus"));
                myForm.ShowDialog();
                //collect result from form
                XLVirtualCabinet.BondResult outcome = myForm.outcome;

                if (outcome.ExitCode == 0)
                {
                    XLDocument.EndDocument();
                    xlTaskPane1.Dispose();
                }
                else
                {
                    MessageBox.Show("Unable to index document, please index manually.  Error code: " + outcome.ExitCode.ToString() + "-" + outcome.StandardOutput.ToString());
                }
                //close the dialog in any event.
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error calling the VC integration. Error code: " + ex.ToString());
                XLtools.LogException("IndexBtn", ex.ToString());
            }
        }

        private void ContactAddressBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                ContForm myForm = new ContForm();
                myForm.ShowDialog();
                XLMain.Contact contact = myForm.selectedContact;
                string str = "";
                if (contact.addresses[0].addressBlock != null)
                {
                    if (contact.salutations.Count > 0)
                    {
                        str += contact.salutations[0].addressee + Environment.NewLine;
                    }
                    str += contact.addresses[0].addressBlock;
                    XLDocument.InsertText(str);
                    //insert the status text box;
                    XLDocument.AddStatusBox();
                    XLDocument.ChangeStatus("Draft");
                }
                else
                {
                    MessageBox.Show(contact.firstname + " " + contact.lastname + " does not have an address in the system.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to get contact address");
                XLtools.LogException("ContactAddressBtn", ex.ToString());
            }
        }

        private void ClientAddressBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                ClientForm myForm = new ClientForm();
                myForm.ShowDialog();
                XLMain.Client client = myForm.selectedClient;
                //Add the name and address at the cursor location
                string str = "";
                if (client.addresses.Count > 0)
                {
                    if (client.salutations.Count > 0) 
                    {
                        str += client.salutations[0].addressee + Environment.NewLine;
                        if (client.name != client.salutations[0].addressee)
                        {
                            str += client.name + Environment.NewLine;
                        }
                    }
                    else
                    {
                        str += client.name + Environment.NewLine;
                    }
                    str += client.addresses[0].addressBlock;
                    XLDocument.InsertText(str);
                    //Add any parameters we can for later indexing
                    string fileStore = XLVirtualCabinet.FileStore(client.office, client.department);
                    XLDocument.UpdateParameter("CRMid", client.crmID);
                    XLDocument.UpdateParameter("Cabinet", fileStore);
                    XLDocument.UpdateParameter("ClientStr", client.clientcode + " - " + client.name);
                    //insert the status text box;
                    XLDocument.AddStatusBox();
                    XLDocument.ChangeStatus("Draft");
                }
                else
                {
                    MessageBox.Show(client.name + " does not have an address in the system.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to client address");
                XLtools.LogException("ClientAddressBtn", ex.ToString());
            }
        }

        private void LaunchClientDoc(string docType)
        {
            try
            {
                ClientForm myForm = new ClientForm();
                myForm.ShowDialog();
                XLMain.Client selectedClient = myForm.selectedClient;
                if (selectedClient != null)
                {
                    string crmId = selectedClient.crmID;
                    XLDocument.openTemplate(docType);
                    XLDocument.UpdateParameter("CRMid", crmId);
                    XLDocument.UpdateParameter("DocType", docType);
                    
                    //this appears to work in 3.5 notwithstanding the reference to Globals
                    xlTaskPane1 = new XLTaskPane();
                    CustomxlTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(xlTaskPane1, "XLant Document Handler", Globals.ThisAddIn.Application.ActiveWindow);
                    CustomxlTaskPane.Visible = true;
                    CustomxlTaskPane.Width = 350;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to launch document");
                XLtools.LogException("LaunchClientDoc", docType + " - " + e.ToString());
            }
        }

        private void StyleBtn_Click(object sender, RibbonControlEventArgs e)
        {
            XLDocument.AddStyles();
        }

        private void ForwardBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                XLMain.Client client = null;
                string fileID = XLDocument.GetFileID();
                if (String.IsNullOrEmpty(fileID))
                {
                    MessageBox.Show("Unable to find FileID.  If the document has not yet been placed in VC click First Index" + Environment.NewLine + "alternatively reindex the document with Virtual Cabinet.");
                    Exception ex = new Exception("Failed to find File ID");
                }

                if (XLDocument.ReadParameter("CRMid") != null)
                {
                    client = XLMain.Client.FetchClient(XLDocument.ReadParameter("CRMid"));
                }
                else
                {
                    //if the document param doesn't exist get the index data from VC
                    client = XLVirtualCabinet.GetClientFromIndex(fileID);
                }
            
                XLMain.Staff writer = new XLMain.Staff();
                string writerID = XLDocument.ReadParameter("Sender");
                if (writerID == "")
                {
                    writer = XLMain.Staff.StaffFromUser(Environment.UserName);
                }
                else
                {
                    writer = XLMain.Staff.StaffFromUser(XLDocument.ReadParameter("Sender"));
                }
                StaffSelectForm myForm = new StaffSelectForm(client, writer);
                myForm.ShowDialog();
                XLMain.EntityCouplet staff = myForm.selectedStaff;

                string commandfileloc = "";
                if (XLDocument.ReadBookmark("Date") == "")
                {
                    commandfileloc = XLVirtualCabinet.Reindex(XLDocument.GetFileID(), staff.name);
                }
                else
                {
                    string docDate = XLDocument.ReadBookmark("Date");
                    commandfileloc = XLVirtualCabinet.Reindex(XLDocument.GetFileID(), staff.name, docDate: docDate);
                }
            
                //MessageBox.Show(commandfileloc);
                XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true);
                if (result.ExitCode != 0)
                {
                    MessageBox.Show("Reindex failed please complete manually.");
                }
                else
                {
                    XLDocument.EndDocument();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to forward document");
                XLtools.LogException("ForwardBtn", ex.ToString());
            }
         }

        private void UpdateDatebtn_Click(object sender, RibbonControlEventArgs e)
        {
            string date = XLDocument.ReadBookmark("Date");
            //check to see if bookmark exists, if not just insert date.
            if (String.IsNullOrEmpty(date)) 
            {
            	XLDocument.InsertText(DateTime.Now.ToString("d MMMM yyyy"));
            }
            else
            {
	            XLDocument.UpdateBookmark("Date", DateTime.Now.ToString("d MMMM yyyy"));
	            XLDocument.UpdateBookmark("Date2", DateTime.Now.ToString("d MMMM yyyy"));
            }
        }

        private void TempSaveBtn_Click(object sender, RibbonControlEventArgs e)
        {
            XLDocument.TempSave();
        }

        private void UpdateRefBtn_Click(object sender, RibbonControlEventArgs e)
        {
            string fileID = XLDocument.GetFileID();
            if (fileID == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                MessageBox.Show("Unable to find FileID.  If the document has not yet been placed in VC click First Index.");
            }
            else
            {
                string ourRef = XLDocument.ReadBookmark("OurRef");
	            //check to see if bookmark exists, if not just insert date.
                if (String.IsNullOrEmpty(ourRef)) 
                {
                	XLDocument.InsertText(fileID);
                }
                else
                {
                	ourRef += @"/" + fileID;
                	XLDocument.UpdateBookmark("OurRef", ourRef);
                }
            }

        }

        private void StaffAddressBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                StaffForm myForm = new StaffForm();
                myForm.ShowDialog();
                XLMain.Staff contact = myForm.selectedContact;
                string str = "";
                if (contact.addresses[0].addressBlock != null)
                {
                    if (contact.salutations.Count > 0)
                    {
                        str += contact.salutations[0].addressee + Environment.NewLine;
                    }
                    str += contact.addresses[0].addressBlock;
                    XLDocument.InsertText(str);
                    //insert the status text box;
                    XLDocument.AddStatusBox();
                    XLDocument.ChangeStatus("Draft");
                }
                else
                {
                    MessageBox.Show(contact.name + " does not have an address in the system.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to get staff address");
                XLtools.LogException("StaffAddressBtn", ex.ToString());
            }
        }

        private void InsolAddressBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                ClientForm myClientForm = new ClientForm();
                myClientForm.ShowDialog();
                XLMain.Client client = myClientForm.selectedClient;
                IPSContForm myForm = new IPSContForm(client);
                myForm.ShowDialog();
                XLInsol.Contact selectContact = myForm.selectedContact;
                if (selectContact != null)
                {
                    string str = selectContact.name + Environment.NewLine;
                    str += selectContact.addressBlock;
                    XLDocument.InsertText(str);
                    XLDocument.AddStatusBox();
                    XLDocument.ChangeStatus("Draft");
                }
                else
                {
                    MessageBox.Show("No contact selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to get insolvency address");
                XLtools.LogException("InsolAddressBtn", ex.ToString());
            }
        }

        private void button2_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {

                //get current user
                XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);
                //Build string and add to document
                string str = user.name + ";";
                str += user.grade + ";";
                str += DateTime.Now.ToString("d MMMM yyyy");
                XLDocument.InsertText(str.Replace(";", Environment.NewLine));

                //insert into document properties with added field
                str += ";" + user.name;
                for (int i = 0; i < 10; i++)
                {
                    string param = "Sign" + i;
                    string s = XLDocument.ReadParameter(param);
                    if (s == "")
                    {
                        XLDocument.UpdateParameter(param, str);
                        break;
                    }
                    if (i == 10)
                    {
                        MessageBox.Show("Signature fields full");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add signature");
                XLtools.LogException("AddSignature", ex.ToString());
            }
        }

        private void SigViewBtn_Click(object sender, RibbonControlEventArgs e)
        {
            sigPane = new SigTaskPane();
            CustomxlTaskPane = Globals.ThisAddIn.CustomTaskPanes.Add(sigPane, "Signature Viewer", Globals.ThisAddIn.Application.ActiveWindow);
            CustomxlTaskPane.Visible = true;
            CustomxlTaskPane.Width = 350;
        }

        private void MLPdfBtn_Click(object sender, RibbonControlEventArgs e)
        {
            //get the id for later use and before the original is closed.
            string fileID = XLDocument.GetFileID();
            //save the document as a pdf and get location
            string file = XLDocument.CreatePdf();
            //close the original, it isn't required any more
            XLDocument.EndDocument();
            //add the header and get the location of the new combined file
            file = XLDocument.AddHeadertoPDF(file);
            //index the combined file using the data from the original
            XLDocument.IndexPDFCopy(file, fileID);
        }

        private void ScannedImage_Click(object sender, RibbonControlEventArgs e)
        {
            //get current user
            XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);
            bool success = XLDocument.AddSignature(user.username);
            if (success)
            {
                success = XLDocument.AddSignatureMetaData(user, user);
            }
        }

        private void MySigBtn_Click(object sender, RibbonControlEventArgs e)
        {
            //get current user
            XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);
            bool success = XLDocument.AddSignature(user.username);
            if (success)
            {
                success = XLDocument.AddSignatureMetaData(user, user);
            }
        }

        private void StaffSig_Click(object sender, RibbonControlEventArgs e)
        {
            //get current user
            XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName); 
            bool auth = AuthCheck(user);
            if (auth)
            {
                StaffForm myForm = new StaffForm();
                myForm.ShowDialog();
                XLMain.Staff contact = myForm.selectedContact;
                //get current user
                bool success = XLDocument.AddSignature(contact.username);
                if (success)
                {
                    success = XLDocument.AddSignatureMetaData(user, contact);
                }
            }
            else
            {
                MessageBox.Show("You do not have authority to use this feature");
            }
        }

        private void MLsignature_Click(object sender, RibbonControlEventArgs e)
        {
            //get current user
            XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);
            bool auth = AuthCheck(user);
            if (auth)
            {
                bool success = XLDocument.AddSignature("MilstedLangdon");
                if (success)
                {
                    XLMain.Staff mL = new XLMain.Staff();
                    mL.username = "MilstedLangdon";
                    mL.name = "Milsted Langdon LLP";
                    success = XLDocument.AddSignatureMetaData(user, mL);
                }
            }
            else
            {
                MessageBox.Show("You do not have authority to use this feature");
            }
        }

        private bool AuthCheck(XLMain.Staff user)
        {
            bool test = false;
            if (user != null)
            {
                if (user.grade.ToUpper() == "PARTNER" || user.grade.ToUpper() == "MANAGER" || user.grade.ToUpper() == "ADMIN")
                {
                    test = true;
                }
            }
            
            return test;
        }

        private void docRefButton_Click(object sender, RibbonControlEventArgs e)
        {
            XLDocument.InsertText("DOCREF: " + XLDocument.currentDoc.FullName);
        }

        private void MergeBtn_Click(object sender, RibbonControlEventArgs e)
        {
            XLForms.StaffSelectForm staffForm = new XLForms.StaffSelectForm();
            staffForm.ShowDialog();
            if (staffForm.selectedStaff != null)
            {
                Document currentDoc = XLDocument.GetCurrentDoc();
                long startPosition = currentDoc.Content.Start;
                long endPosition = currentDoc.Content.End;
                string templateXML = XLDocument.CopyRangeToWordXML(currentDoc.Range());
                List<XLMain.FPIClient> clients = XLMain.FPIClient.GetFPIClients(staffForm.selectedStaff.crmID, "where Last_year = 0");
                if (clients.Count > 0)
                {
                    XLDocument.MergeFPIData(clients.OrderBy(c => c.office).ToList(), templateXML);
                }
                else
                {
                    MessageBox.Show("No clients founds to merge.");
                }
                clients = XLMain.FPIClient.GetFPIClients(staffForm.selectedStaff.crmID, "where Last_year = 0", false);
                if (clients.Count > 0)
                {
                    XLDocument.MergeFPIData(clients.OrderBy(c => c.office).ToList(), templateXML);
                }
                else
                {
                    MessageBox.Show("No foreign clients founds to merge.");
                }
            }
        }

        private void Merge2Btn_Click(object sender, RibbonControlEventArgs e)
        {
            XLForms.StaffSelectForm staffForm = new XLForms.StaffSelectForm();
            staffForm.ShowDialog();
            Document currentDoc = XLDocument.GetCurrentDoc();
            long startPosition = currentDoc.Content.Start;
            long endPosition = currentDoc.Content.End;
            string templateXML = XLDocument.CopyRangeToWordXML(currentDoc.Range());
            List<XLMain.FPIClient> clients = XLMain.FPIClient.GetFPIClients(staffForm.selectedStaff.crmID, "where direct_debit = 1 and Last_year = 1");
            if (clients.Count > 0)
            {
                XLDocument.MergeFPIData(clients.OrderBy(c => c.office).ToList(), templateXML);
            }
            else
            {
                MessageBox.Show("No clients founds to merge.");
            }
            clients = XLMain.FPIClient.GetFPIClients(staffForm.selectedStaff.crmID, "where direct_debit = 1 and Last_year = 1", false);
            if (clients.Count > 0)
            {
                XLDocument.MergeFPIData(clients.OrderBy(c => c.office).ToList(), templateXML);
            }
            else
            {
                MessageBox.Show("No foreign clients founds to merge.");
            }
        }

        private void Merge3Btn_Click(object sender, RibbonControlEventArgs e)
        {
            XLForms.StaffSelectForm staffForm = new XLForms.StaffSelectForm();
            staffForm.ShowDialog();
            Document currentDoc = XLDocument.GetCurrentDoc();
            long startPosition = currentDoc.Content.Start;
            long endPosition = currentDoc.Content.End;
            string templateXML = XLDocument.CopyRangeToWordXML(currentDoc.Range());
            List<XLMain.FPIClient> clients = XLMain.FPIClient.GetFPIClients(staffForm.selectedStaff.crmID, "where direct_debit = 0 and Last_year = 1");
            if (clients.Count > 0)
            {
                XLDocument.MergeFPIData(clients.OrderBy(c => c.office).ToList(), templateXML);
            }
            else
            {
                MessageBox.Show("No clients founds to merge.");
            }
            clients = XLMain.FPIClient.GetFPIClients(staffForm.selectedStaff.crmID, "where direct_debit = 0 and Last_year = 1", false);
            if (clients.Count > 0)
            {
                XLDocument.MergeFPIData(clients.OrderBy(c => c.office).ToList(), templateXML);
            }
            else
            {
                MessageBox.Show("No foreign clients founds to merge.");
            }
        }

        private void Merge4Btn_Click(object sender, RibbonControlEventArgs e)
        {
            XLForms.ClientForm clientForm = new XLForms.ClientForm();
            clientForm.ShowDialog();

            if (clientForm.selectedClient != null)
            {
                XLMain.Client client = clientForm.selectedClient;
                List<XLMain.FPIClient> clients = XLMain.FPIClient.GetFPIClients(client);
                if (clients.Count > 0)
                {
                    XLDocument.MergeFPIData(clients.OrderBy(c => c.office).ToList());
                }
                else
                {
                    MessageBox.Show("No clients founds to merge.");
                } 
            }
        }

        private void InvoiceBtn_Click(object sender, RibbonControlEventArgs e)
        {
            XLForms.ClientForm clientForm = new XLForms.ClientForm();
            clientForm.ShowDialog();

            if (clientForm.selectedClient != null)
            {
                XLMain.Client client = clientForm.selectedClient;
                XLMain.FPIClient fpiClient = XLMain.FPIClient.GetFPIClientInvoice(client);
                List<XLMain.FPIClient> clientList = new List<XLMain.FPIClient>();
                clientList.Add(fpiClient);
                XLDocument.MergeFPIData(clientList.OrderBy(c => c.office).ToList());
            }
        }
    }
}
