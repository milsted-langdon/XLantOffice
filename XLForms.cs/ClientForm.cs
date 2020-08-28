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

namespace XLForms
{
    public partial class ClientForm : Form
    {
        public XLMain.Client selectedClient;
        private List<XLMain.Client> clientList;

        public ClientForm(string initialQuery="", string param1=null, string param2=null, List<XLMain.Client> recentList = null)
        {
            InitializeComponent();
            this.CenterToParent();
            if (initialQuery != "")
            {
                Search(initialQuery, param1, param2);
            }
            if (recentList != null)
            {
                if (recentList.Count > 0)
                {
                    //pass to local variable
                    clientList = recentList;

                    RecentClientListBox.DisplayMember = "Name";
                    RecentClientListBox.ValueMember = "crmid";
                    foreach (XLMain.Client client in clientList)
                    {
                        XLMain.EntityCouplet newEntity = new XLMain.EntityCouplet();
                        newEntity.crmID = client.crmID;
                        newEntity.name = client.name;
                        RecentClientListBox.Items.Add(newEntity);
                    }
                }
                else
                {
                    RecentClientListBox.Visible = false;
                }
            }
        }

        private void Search(string query="", string param1=null, string param2=null)
        {
            //clear existing entries if any
            ClientListBox.Items.Clear();

            //start a new search
            string searchStr = SearchTB.Text;
            Boolean IncLost = IncLostCheck.Checked;
            ClientListBox.DisplayMember = "Name";
            ClientListBox.ValueMember = "crmid";
            DataTable xlReader = null;

            if (query == "")
            {
                query = "Select clientcode + ' - ' + name as Name, crmID from Client where ((ClientCode like '%' + @param1 + '%') Or (Name Like  '%' + @param1 + '%'))";
                param1 = searchStr;
                if (!IncLost)
                {
                    query += " and status in ('New', 'Active')";
                }
                query += " order by name";
            }
            
            xlReader = XLSQL.ReturnTable(query, param1, param2);

            if (xlReader != null)
            {
                if (xlReader.Rows.Count!=0)
                {
                    foreach (DataRow row in xlReader.Rows)
                    {
                        XLMain.EntityCouplet newEntity = new XLMain.EntityCouplet();
                        newEntity.crmID = row["Crmid"].ToString();
                        newEntity.name = row["name"].ToString();
                        ClientListBox.Items.Add(newEntity);
                    }
                }
                else
                {
                    MessageBox.Show("No Records found");
                }
            }
            else
            {
                MessageBox.Show("Unable to connect to the database.");
            }
            
        }

        private void SearchBtn_Click_1(object sender, EventArgs e)
        {
            Search();
        }

        private void ClientListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            XLMain.EntityCouplet selectedItem = (XLMain.EntityCouplet)ClientListBox.SelectedItem;
            selectedClient = XLMain.Client.FetchClient(selectedItem.crmID);
            this.Close();
        }

        private void SearchTB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                Search();
            }
        }

        private void RecentClientListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            XLMain.EntityCouplet selectedItem = (XLMain.EntityCouplet)RecentClientListBox.SelectedItem;
            //to avoid another call to the database, pull from the list instead
            selectedClient = clientList.Where(c => c.crmID == selectedItem.crmID).First();
            this.Close();
        }
    }
}

