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
    public partial class ForwardForm : Form
    {
        public XLMain.Staff selectedStaff;

        public ForwardForm()
        {
            InitializeComponent();
            this.CenterToParent();
            XLMain.Client client = new XLMain.Client();
            XLMain.Staff staff = new XLMain.Staff();

            //populate To Be Actioned by
            staff = XLMain.Staff.StaffFromUser(XLDocument.ReadParameter("Sender"));
            string str = XLDocument.ReadParameter("CRMid");

            client = XLMain.Client.FetchClient(str);

            //populate the sender
            List<XLMain.Staff> users = new List<XLMain.Staff>();
            users = XLMain.Staff.AllStaff();

            //place current and connected users at the top of the list, could remove them elsewhere but seems no point
            if (staff != null)
            {
                users.Insert(0, staff);
            }
            if (client.partner != null)
            {
                users.Insert(1, client.partner);
            }
            if (client.manager != null)
            {
                users.Insert(2, client.manager);
            }

            //users.Insert(3, client.other);  Other not yet part of client
            ToBeActionDDL.DataSource = users;
            ToBeActionDDL.DisplayMember = "name";
            ToBeActionDDL.ValueMember = "crmID";
            if (staff != null)
            {
                ToBeActionDDL.SelectedItem = staff;
            }
        }

        private void IndexBtn_Click(object sender, EventArgs e)
        {
            selectedStaff = (XLMain.Staff)ToBeActionDDL.SelectedItem;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
