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
    public partial class SelectForm : Form
    {
        public XLMain.Contact selectedContact;
        private XLMain.Client client;

        public SelectForm(List<XLMain.EntityCouplet> ContList, XLMain.Client passedClient)
        {
            InitializeComponent();
            this.CenterToParent();
            client = passedClient;
            ContactListBox.DisplayMember = "value";
            ContactListBox.ValueMember = "crmId";
            ContactListBox.DataSource = ContList;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            foreach (object item in ContactListBox.SelectedItems)
            {
                XLMain.EntityCouplet couplet = (XLMain.EntityCouplet)item;
                List<SqlParameter> paramCollection = new List<SqlParameter>();
                paramCollection.Add(new SqlParameter("ClientCRM", client.crmID));
                paramCollection.Add(new SqlParameter("ContactCRM", couplet.crmID));
                XLSQL.RunCommand("ConnectionCreation", paramCollection);
            }
            this.Close();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

