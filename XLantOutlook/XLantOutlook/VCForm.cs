using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XLant;

namespace XLantOutlook
{
    public partial class VCForm : Form
    {
        private static XLMain.Client client = new XLMain.Client();
        private static MailItem email = new MailItem();

        public VCForm(XLMain.Client recClient, MailItem recEmail)
        {
            InitializeComponent();
            client = recClient;
            email = recEmail;

            //Get the information you can
            //Collect data for filing
            string desc = email.SenderName + "re:" + email.Subject;
            DescTB.Text = desc;

            //populate To Be Actioned by
            XLMain.Staff staff = new XLMain.Staff();
            staff = XLMain.Staff.StaffFromUser(Environment.UserName);
            
            //populate the sender
            List<XLMain.Staff> users = new List<XLMain.Staff>();
            users = XLMain.Staff.AllStaff();
			int i = 0;
            //place current and connected users at the top of the list, could remove them elsewhere but seems no point
            if (staff != null)
            {
                users.Insert(i, staff);
                i += 1;
            }
            if (client.partner != null)
            {
                users.Insert(i, client.partner);
                i += 1;
            }
            if (client.manager != null)
            {
                users.Insert(i, client.manager);
                i += 1;
            }
            //add a blank in case there is no further action required
            users.Insert(i, new XLMain.Staff());

            //users.Insert(3, client.other);  Other not yet part of client
            ToBeActionDDL.DataSource = users;
            ToBeActionDDL.DisplayMember = "name";
            ToBeActionDDL.ValueMember = "crmID";
            if (staff != null)
            {
                ToBeActionDDL.SelectedItem = staff;
            }
            
            if (client.department!="INS")
            {
                FileSectionDDL.Visible = false;
                FileSectionlbl.Visible = false;
            }
            else
            {
                FileSectionDDL.Visible = true;
                FileSectionlbl.Visible = true;
            }
        }

        private void IndexBtn_Click(object sender, EventArgs e)
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\XLant\\temp\\";
            string tempPath = folder + "temp.msg";

            email.SaveAs(tempPath);

            string cabinet = XLVirtualCabinet.FileStore(client.office, client.department);
            string clientStr = client.clientcode + " - " + client.name;
            string status = "";
            if (email.ReceivedTime != null)
            {
                status = "External";
            }
            else if (email.SentOn != null)
            {
                status = "Sent";
            }
            else
            {
                status = "Draft";
            }
            XLMain.Staff toBe = (XLMain.Staff)ToBeActionDDL.SelectedItem;
            string desc = DescTB.Text;
            string section ="";
            if (client.department!="INS")
            {
                section = "Correspondence";
            }
            else
            {
                section = FileSectionDDL.SelectedItem.ToString();
            }
            //Launch the index process and collect the result
            XLVirtualCabinet.BondResult outcome = XLVirtualCabinet.IndexDocument(tempPath, cabinet, clientStr, status, toBe.name, section, desc);
             
            //close the dialog in any event.
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
