using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLForms
{
    public partial class DateRangeForm : Form
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public DateRangeForm()
        {
            InitializeComponent();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            FromDate = FromDatePicker.Value;
            ToDate = ToDatePicker.Value;
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
