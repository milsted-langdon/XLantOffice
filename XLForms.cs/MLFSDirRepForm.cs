using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XLantCore.Models;

namespace XLForms
{
    public partial class MLFSDirRepForm : Form
    {
        

        public MLFSDirRepForm(List<MLFSReportingPeriod> repPeriods)
        {
            NewPeriods = new List<MLFSReportingPeriod>();
            Periods = new List<MLFSReportingPeriod>();
            InitializeComponent();
            Periods = repPeriods;
            PeriodDDL.DataSource = Periods;
            PeriodDDL.DisplayMember = "Description";
            PeriodDDL.ValueMember = "Id";
        }

        public string FeesFile { get; set; }
        public string PlansFile { get; set; }
        public string FCIFile { get; set; }
        public string PeriodId { get; set; }
        private List<MLFSReportingPeriod> Periods { get; set; }
        public List<MLFSReportingPeriod> NewPeriods { get; set; }


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
            PeriodId = PeriodDDL.SelectedValue.ToString();
            this.Close();
        }

        private void ChooseFCIFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowReadOnly = true;
            fileDialog.Filter = "CSV Files|*.csv";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                FCIFileTb.Text = fileDialog.FileName;
                FCIFile = fileDialog.FileName;
            }
        }

        private void addPeriodBtn_Click(object sender, EventArgs e)
        {
            MLFSReportingPeriodAdd addForm = new MLFSReportingPeriodAdd(Periods);
            addForm.ShowDialog();
            if (addForm.newPeriod != null)
            {
                MLFSReportingPeriod period = addForm.newPeriod;
                NewPeriods.Add(period);
                Periods.Add(period);
            }
            
        }
    }
}
