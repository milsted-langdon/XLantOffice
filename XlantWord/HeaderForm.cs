using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XlantWord
{
    public partial class HeaderForm : Form
    {
        public HeaderForm()
        {
            InitializeComponent();
            this.CenterToParent();
            OfficeDDL.DataSource = XLDocument.GetList("Office");
            OfficeDDL.DisplayMember = "name";
            OfficeDDL.ValueMember = "dbTag";
            DeptDDL.DataSource = XLDocument.GetList("Footer");
            DeptDDL.DisplayMember = "name";
            DeptDDL.ValueMember = "dbTag";
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            XLDocument.Header header = new XLDocument.Header();
            string office = (string)OfficeDDL.SelectedValue;
            string footer = (string)DeptDDL.SelectedValue;
            header = XLDocument.MapHeaderFooter(office, footer);
            if (header != null)
            {
                XLDocument.DeployHeader(header);
                XLDocument.UpdateParameter("HeaderDeployed", "true");
            }
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
