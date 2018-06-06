using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XLant;

namespace XLForms
{
    public partial class DescForm : Form
    {
        public string description;
        public string toBeActioned;
        
        public DescForm(string senderUsername = "", string tempDesc = "")
        {
            
            InitializeComponent();
            this.CenterToParent();
            DescTB.Text = tempDesc;

            XLMain.Staff user = XLMain.Staff.StaffFromUser(senderUsername);
            //populate the ddl
            List<XLMain.Staff> users = new List<XLMain.Staff>();
            users = XLMain.Staff.AllStaff();
            //place the sender at the top of the list
            if (user != null)
            {
                users.Insert(0, user);
            }
            SenderDDL.DataSource = users;
            SenderDDL.DisplayMember = "name";
            SenderDDL.ValueMember = "crmID";
            SenderDDL.SelectedItem = user;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            description = DescTB.Text;
            XLMain.Staff user = (XLMain.Staff)SenderDDL.SelectedItem;
            toBeActioned = user.name;
            this.Close();
        }
    }
}
