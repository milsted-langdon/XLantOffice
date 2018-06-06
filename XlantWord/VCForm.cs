using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XLant;

namespace XlantWord
{
    public partial class VCForm : Form
    {
        private XLMain.Client client = new XLMain.Client();
        private string docPath = null;
        private string status = null;

        public VCForm(XLMain.Staff writer, XLMain.Client client, string docPath, string desc ="", string status="Draft")
        {
            InitializeComponent();
            this.CenterToParent();
            
            XLMain.Staff user = XLMain.Staff.StaffFromUser(Environment.UserName);

            DescTB.Text = desc;   

            //populate the sender
            List<XLMain.Staff> users = XLtools.StaffList(user, client, true);
            
            ToBeActionDDL.DataSource = users;
            ToBeActionDDL.DisplayMember = "name";
            ToBeActionDDL.ValueMember = "crmID";
            if (writer != null)
            {
                ToBeActionDDL.SelectedItem = writer;
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
            string fullPath = docPath;
            string cabinet = XLVirtualCabinet.FileStore(client.manager.office, client.department);
            string clientStr = client.clientcode + " - " + client.name;
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
            XLVirtualCabinet.BondResult outcome = XLVirtualCabinet.IndexDocument(fullPath, cabinet, clientStr, status, toBe.name, section, desc);
            
            if (outcome.ExitCode == 0)
            {
                XLDocument.EndDocument();
            }
            else
            {
                MessageBox.Show("Unable to index document, please index manually.  Error code: " + outcome.ExitCode.ToString() + "-" + outcome.StandardOutput.ToString());
            }
            //close the dialog in any event.
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
