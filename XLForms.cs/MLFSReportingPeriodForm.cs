using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XLantCore.Models;

namespace XLForms
{
    public partial class MLFSReportingPeriodForm : Form
    {
        public int? PeriodId { get; set; }

        public MLFSReportingPeriodForm(List<MLFSReportingPeriod> repPeriods)
        {
            InitializeComponent();
            PeriodDDL.DataSource = repPeriods;
            PeriodDDL.DisplayMember = "Description";
            PeriodDDL.ValueMember = "Id";
 
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            PeriodId = null;
            this.Close();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            PeriodId = (int)PeriodDDL.SelectedValue;
            this.Close();
        }
    }
}
