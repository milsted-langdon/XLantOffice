using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class MLFSIncomeReport
    {

        public MLFSIncomeReport(MLFSIncome income) 
        {
            Reporting_Period = income.ReportingPeriod.Description;
            if (income.Advisor == null)
            {
                Advisor = "Unknown";
            }
            else
            {
                Advisor = income.Advisor.Fullname; 
            }
            Organisation = income.Organisation;
            Campaign = income.Campaign;
            if (income.IsNew)
            {
                New_Amount = income.Amount;
                Recurring_Amount = 0;
            }
            else
            {
                New_Amount = 0;
                Recurring_Amount = income.Amount;
            }
        }
        public string Reporting_Period { get; set; }
        public string Advisor { get; set; }
        public string Organisation { get; set; }
        public string Campaign { get; set; }
        public string IsNew { get; set; }
        public decimal New_Amount { get; set; }
        public decimal Recurring_Amount { get; set; }
    }
}
