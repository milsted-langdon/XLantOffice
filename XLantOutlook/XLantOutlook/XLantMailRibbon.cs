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
    public partial class XLantMailRibbon
    {
        private void XLantRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void IndexBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = ThisEmail();
            Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            string path = "";
            path = explorer.CurrentFolder.FolderPath;
            
            XLOutlook.IndexEmail(email, path);
            email.Save();
            explorer.CurrentFolder.CurrentView.Apply();
        }

        private void ForwardBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = ThisEmail();
            email.Save();
            XLMain.Client client = XLMain.Client.FetchClient(XLOutlook.ReadParameter("CrmID", email));
            XLMain.Staff writer = XLMain.Staff.StaffFromUser(Environment.UserName);
            if (XLantRibbon.staff.Count == 0)
            {
                XLantRibbon.staff = XLMain.Staff.AllStaff();
            }
            StaffSelectForm myForm = new StaffSelectForm(client, writer, XLantRibbon.staff);
            myForm.ShowDialog();
            XLMain.EntityCouplet staff = myForm.selectedStaff;

            string commandfileloc = "";

            string fileId = XLOutlook.ReadParameter("VCFileID", email);
            commandfileloc = XLVirtualCabinet.Reindex(fileId, staff.name, docDate: DateTime.Now.ToString("dd/MM/yyyy"));

            XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true);
            if (result.ExitCode != 0)
            {
                MessageBox.Show("Reindex failed please complete manually.");
            }
            else
            {
                // Close the email in Outlook to prevent further changes that won't be saved to VC
                email.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olSave);
                // Delete the email from the Drafts folder in Outlook
                email.Delete();
            }
        }

        private void ApproveBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = ThisEmail();
            email.Save();
            XLMain.Client client = XLMain.Client.FetchClient(XLOutlook.ReadParameter("CrmID", email));
            XLMain.Staff writer = XLMain.Staff.StaffFromUser(Environment.UserName);
            if (XLantRibbon.staff.Count == 0)
            {
                XLantRibbon.staff = XLMain.Staff.AllStaff();
            }
            StaffSelectForm myForm = new StaffSelectForm(client, writer, XLantRibbon.staff);
            myForm.ShowDialog();
            XLMain.EntityCouplet staff = myForm.selectedStaff;

            string commandfileloc = "";

            string fileId = XLOutlook.ReadParameter("VCFileID", email);
            commandfileloc = XLVirtualCabinet.Reindex(fileId, staff.name, status: "Approved", docDate: DateTime.Now.ToString("dd/MM/yyyy"));

            XLVirtualCabinet.BondResult result = XLVirtualCabinet.LaunchCabi(commandfileloc, true);
            if (result.ExitCode != 0)
            {
                MessageBox.Show("Reindex failed please complete manually.");
            }
            else
            {
                // Close the email in Outlook to prevent further changes that won't be saved to VC
                email.Close(Microsoft.Office.Interop.Outlook.OlInspectorClose.olSave);
                // Delete the email from the Drafts folder in Outlook
//                email.Delete();
            }
        }

        private void StartBtn_Click(object sender, RibbonControlEventArgs e)
        {
            XLOutlook.IndexDraft(ThisEmail());

        }

        private Outlook.MailItem ThisEmail()
        {
            Outlook.Application application = Globals.ThisAddIn.Application;
            Outlook.Inspector inspector = application.ActiveInspector();
            Outlook.MailItem myMailItem = (Outlook.MailItem)inspector.CurrentItem;
            return myMailItem;
        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show(XLOutlook.ReadParameter("VCFileID", ThisEmail()));
        }

        private void QuickIndex_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = ThisEmail();
            Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            string path = "";
            path = explorer.CurrentFolder.FolderPath;

            XLOutlook.QuickIndex(email, path);
            explorer.CurrentFolder.CurrentView.Apply();
        }

        private void MultiClientIndexBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = ThisEmail();
            Outlook.Explorer explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            string path = "";
            path = explorer.CurrentFolder.FolderPath;

            XLOutlook.MultiQuickIndex(email, path);
            explorer.CurrentFolder.CurrentView.Apply();
        }

        private void IndexAttachmentBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MailItem email = ThisEmail();
            XLOutlook.IndexAttachments(email);
        }





    }
}
