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
    public partial class StaffForm : Form
    {
        public XLMain.Staff selectedContact;

        public StaffForm(string initialQuery = "", string param1 = null, string param2 = null)
        {
            InitializeComponent();
            this.CenterToParent();
            if (initialQuery != "")
            {
                Search(initialQuery, param1, param2);
            }
        }

        private void Search(string query="", string param1 = null, string param2 = null)
        {
            //clear existing entries if any
            ContactListBox.Items.Clear();
            
            //start a new search
            string searchStr = SearchTB.Text;
            ContactListBox.Items.Clear();
            ContactListBox.DisplayMember = "Name";
            ContactListBox.ValueMember = "crmid";
            DataTable xlReader = null;

            if (query == "")
            {
                query = "select Fullname + ' - ' + Department as display, CRMid from Staff where (Fullname like '%' + @param1 + '%') order by surname";
                param1 = searchStr;
            }

            xlReader = XLSQL.ReturnTable(query, param1, param2);

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
            Search();
        }

        private void ContactListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            XLMain.EntityCouplet selectedItem = (XLMain.EntityCouplet)ContactListBox.SelectedItem;
            selectedContact = XLMain.Staff.FetchStaff(selectedItem.crmID);
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

