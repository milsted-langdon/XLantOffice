using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XLantCore;

namespace XLantExcel
{
    public partial class RawDataForm : Form
    {
        public RawDataForm(string requested)
        {
            InitializeComponent();
            Requested = requested;
            System.Data.DataTable viewQuery = GetViews(requested);
            ViewDDL.DataSource = viewQuery;
            ViewDDL.ValueMember = "name";
            ViewDDL.DisplayMember = "name";
        }

        public DataTable ReturnedTable { get; set; }
        public string Requested { get; set; }

        private System.Data.DataTable GetViews(string requested)
        {
            System.Data.DataTable table = XLSQL.ReturnTable("SELECT Substring(name, 10, LEN(name)-6) AS name FROM sys.views where name like 'Excel_" + requested + "_%'");
            return table;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            ReturnedTable = null;
            this.Close();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            ReturnedTable = XLSQL.ReturnTable("Select * from Excel_" + Requested + "_"  + ViewDDL.SelectedValue);
            this.Close();
        }
    }
}
