using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using XLant;

namespace XlantWord
{
    public partial class ContForm : Form
    {
        public XLMain.Contact selectedContact;
        
        public class EntityCouplet
        {
            public string crmID { get; set; }
            public string name { get; set; }
        }

        public ContForm()
        {
            InitializeComponent();
            this.CenterToParent();
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
            SqlDataReader xlReader = null;

            xlReader = XLSQL.ReaderQuery(query);

            if (xlReader.HasRows)
            {
                while (xlReader.Read())
                {
                    EntityCouplet newEntity = new EntityCouplet();
                    newEntity.crmID = xlReader.NiceString("CRMId");
                    newEntity.name = xlReader.NiceString("display");
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
            string searchStr = SearchTB.Text;
            Search("select Fullname + ' - ' + Case when ISNULL(Organisations.Name,'') = '' then ISNULL(Occupation,'') else Organisations.Name end as display, contact.CRMid from Contact left outer join organisations on contact.organisationid=organisations.crmid where ((Fullname like '%" + searchStr + "%') or (Name like '%" + searchStr + "%')) order by last_name");
        }

        private void ContactListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EntityCouplet selectedItem = (EntityCouplet)ContactListBox.SelectedItem;
            selectedContact = XLMain.Contact.FetchContact(selectedItem.crmID);
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

