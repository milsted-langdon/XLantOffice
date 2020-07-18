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
    public partial class MLFSReportingPeriodAdd : Form
    {
        private readonly List<MLFSReportingPeriod> _currentPeriods;
        public MLFSReportingPeriod newPeriod = new MLFSReportingPeriod();
        public MLFSReportingPeriodAdd(List<MLFSReportingPeriod> currentPeriods)
        {
            InitializeComponent();
            _currentPeriods = currentPeriods;
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            List<Tuple<int, string>> keyValues = new List<Tuple<int, string>>();
            for (int i = 1; i <= 12; i++)
            {
                keyValues.Add(new Tuple<int, string>(i, months[i-1]));
            }
            MonthDDL.DataSource = keyValues;
            MonthDDL.DisplayMember = "Item2";
            MonthDDL.ValueMember = "Item1";
            string[] years = new string[4];
            for (int i = -1; i < 3 ; i++)
            {
                years[i+1] = DateTime.Now.AddYears(i).Year.ToString();
            }
            YearDDL.DataSource = years;
        }

        private void MonthDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateValues();
        }

        private void YearDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateValues();
        }

        private void DescriptionTb_TextChanged(object sender, EventArgs e)
        {
            newPeriod.Description = DescriptionTb.Text; 
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            newPeriod = null;
            this.Close();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CalculateValues()
        {

            if (MonthDDL.SelectedValue != null && YearDDL.SelectedValue != null)
            {
                //Tuple<int, string> selectedMonth = (Tuple<int, string>)MonthDDL.SelectedValue;
                int month = (int)MonthDDL.SelectedValue;
                string monthName = MonthDDL.Text;
                int year = int.Parse(YearDDL.SelectedValue.ToString());
                newPeriod.Month = month;
                newPeriod.Year = year;
                newPeriod.Description = monthName + " " + year.ToString();
                if (month > 4)
                {
                    newPeriod.ReportOrder = month - 4;
                }
                else
                {
                    newPeriod.ReportOrder = month + 8;
                }
                DescriptionTb.Text = newPeriod.Description;
                ReportOrderTb.Value = newPeriod.ReportOrder;

            }
        }
    }
}
