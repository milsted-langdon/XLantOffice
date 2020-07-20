using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using XLantCore;
using XLantCore.Models;

namespace XLForms
{
    public partial class MLFSDirRepForm : Form
    {
        

        public MLFSDirRepForm(List<MLFSReportingPeriod> repPeriods)
        {
            Periods = new List<MLFSReportingPeriod>();
            InitializeComponent();
            Periods = repPeriods;
            BindPeriods();
            AddingNew = false;
        }

        private void BindPeriods()
        {
            PeriodDDL.DataSource = Periods;
            PeriodDDL.DisplayMember = "Description";
            PeriodDDL.ValueMember = "Id";
        }

        public string FeesFile { get; set; }
        public string PlansFile { get; set; }
        public string FCIFile { get; set; }
        public string PeriodId { get; set; }
        public bool AddingNew { get; set; }
        private List<MLFSReportingPeriod> Periods { get; set; }


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

        private void AddNewBtn_Click(object sender, EventArgs e)
        {
            //Call the Process.Start method to open the default browser
            //with a URL:
            string url = APIAccess.baseURL;
            url = url.Substring(0, url.LastIndexOf("/"));
            url += "/MLFSReportingPeriods/Create";
            AddingNew = true;
            RefreshBtn.Visible = true;
            Process.Start("explorer.exe", url);
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            APIAccess.Result result = APIAccess.GetDataFromXLAPI<List<MLFSReportingPeriod>>("/MLFSReportingPeriod/GetCurrent");
            Periods = (List<MLFSReportingPeriod>)result.Data;
            BindPeriods();
            AddingNew = false;
        }
    }
}
