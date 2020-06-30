using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XLant;

namespace XLantExcel
{
    public partial class MLFSDirRepForm : Form
    {

        public MLFSDirRepForm()
        {
            InitializeComponent();
        }

        public string FeesFile { get; set; }
        public string PlansFile { get; set; }
        public DateTime PeriodDate { get; set; }
        public decimal InitialTarget { get; set; }
        public decimal TrailTarget { get; set; }

        private void ChooseFeeFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowReadOnly = true;
            fileDialog.Filter = "CSV Files|*.csv";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                FeesFileTb.Text = fileDialog.FileName;
                FeesFile = fileDialog.FileName;
            }
        }

        private void ChoosePlansFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowReadOnly = true;
            fileDialog.Filter = "CSV Files|*.csv";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                PlansFileTb.Text = fileDialog.FileName;
                PlansFile = fileDialog.FileName;
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            PeriodDate = DateTb.Value;
            InitialTarget = XLtools.HandleNull(InitialTb.Text);
            TrailTarget = XLtools.HandleNull(TrailTargetTb.Text);
            this.Close();
        }
    }
}
