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
    public partial class StaffSelectForm : Form
    {
        public XLMain.EntityCouplet selectedStaff;

        public StaffSelectForm(XLMain.Client client = null, XLMain.Staff sender = null, List<XLMain.EntityCouplet> staffList = null)
        {
            InitializeComponent();
            this.CenterToParent();
            
            //populate the sender
            List<XLMain.EntityCouplet> users = new List<XLMain.EntityCouplet>();
            users = XLtools.StaffList(sender, client, true, userList: staffList);

            SelectDDL.DataSource = users;
            SelectDDL.DisplayMember = "name";
            SelectDDL.ValueMember = "crmID";
            if (sender != null)
            {
                SelectDDL.SelectedItem = sender.crmID;
            }
        }

        private void IndexBtn_Click(object sender, EventArgs e)
        {
            selectedStaff = (XLMain.EntityCouplet)SelectDDL.SelectedItem;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
