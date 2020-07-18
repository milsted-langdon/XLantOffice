using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using XLantCore;

namespace XLForms
{
    public partial class IPSContForm : Form
    {
        public XLInsol.Contact selectedContact;
        private XLMain.Client client = null;
        
        public IPSContForm(XLMain.Client passClient, string initialQuery="")
        {
            InitializeComponent();
            this.CenterToParent();

            if (passClient != null)
            {
                client = passClient;
                CaseCodeTb.Text = client.clientcode;
            }
            if (initialQuery != "")
            {
                Search(initialQuery);
            }
        }

        private void Search(string query)
        {
            //clear existing entries if any
            ContactListBox.Items.Clear();
            
            //start a new search
            string searchStr = SearchTB.Text;
            ContactListBox.Items.Clear();
            ContactListBox.DisplayMember = "Name";
            ContactListBox.ValueMember = "crmid";
            DataTable xlReader = null;

            xlReader = XLSQL.ReturnTable(query);

            if (xlReader.Rows.Count != 0)
            {
                foreach (DataRow row in xlReader.Rows)
                {
                    XLMain.EntityCouplet newEntity = new XLMain.EntityCouplet();
                    newEntity.crmID = row["CRMId"].ToString();
                    newEntity.name = row["display"].ToString();
                    ContactListBox.Items.Add(newEntity);

                }
            }
            else
            {
                MessageBox.Show("No Records found");
            }
        }

        private void SearchBtn_Click_1(object sender, EventArgs e)
        {
            if (client == null || client.clientcode != CaseCodeTb.Text)
            {
                client = XLMain.Client.FetchClientFromCode(CaseCodeTb.Text);
            }
            string searchStr = SearchTB.Text;
            
            Search("select name + ' - ' + address1 as display, id as CRMid from IPSContact('" + client.crmID + "') where name like '%" + searchStr + "%'");
        }

        private void ContactListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            XLMain.EntityCouplet selectedItem = (XLMain.EntityCouplet)ContactListBox.SelectedItem;
            selectedContact = XLInsol.Contact.FetchContact(selectedItem.crmID, client.crmID);
            this.Close();
        }

        private void AddressForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    SearchBtn_Click_1(this,e);
                }
        }

    }
}

