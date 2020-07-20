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
        
        }

        public MLFSReportingPeriod(int month, int year)
        {
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
    }
}
