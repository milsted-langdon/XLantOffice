using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace XLantCore.Models
{
    public partial class MLFSReportingPeriod
    {
        public MLFSReportingPeriod()
        {
            Sales = new List<MLFSSale>();
            Receipts = new List<MLFSIncome>();
            Budgets = new List<MLFSBudget>();
        }

        public MLFSReportingPeriod(int month, int year)
        {
            Sales = new List<MLFSSale>();
            Receipts = new List<MLFSIncome>();
            Budgets = new List<MLFSBudget>();
            Dictionary<int, string> months = Tools.MonthsList();
            string monthName = months[month];
            Month = month;
            Year = year;
            Description = monthName + " " + year.ToString();
            if (month > 4)
            {
                ReportOrder = month - 4;
                FinancialYear = Year.ToString().Substring(2);
                FinancialYear += "/" + (Year + 1).ToString().Substring(2);
            }
            else
            {
                ReportOrder = month + 8;
                FinancialYear = (Year - 1).ToString().Substring(2);
                FinancialYear += "/" + Year.ToString().Substring(2);
            }
            
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        [Display(Name="Financial Year")]
        public string FinancialYear { get; set; }
        [Display(Name = "Report Order")]
        public int ReportOrder { get; set; }
        public bool Locked { get; set; }

        public virtual List<MLFSSale> Sales { get; set; }
        public virtual List<MLFSIncome> Receipts { get; set; }
        public virtual List<MLFSBudget> Budgets { get; set; }
    }
}
