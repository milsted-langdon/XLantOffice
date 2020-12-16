using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XLantCore;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class FCIReport
    {
        public FCIReport()
        {

        }

        [Display(Name = "Advisor")]
        public string Advisor { get; set; }
        [Display(Name = "Organisation")]
        public string Organisation { get; set; }
        [Display(Name = "Ad-hoc Fee")]
        public decimal Adhoc { get; set; }
        [Display(Name = "Fund based Fee")]
        public decimal FundBased { get; set; }
        [Display(Name = "Initial Fee")]
        public decimal Initial { get; set; }
        [Display(Name = "On-going Fee")]
        public decimal Ongoing { get; set; }
        [Display(Name = "Renewal Fee")]
        public decimal Renewal { get; set; }
        [Display(Name = "Any Other Fee")]
        public decimal Other { get; set; }
        [Display(Name = "Total")]
        public decimal Total { get; set; }


        /// <summary>
        /// Creates a of the MLFSIncome objects
        /// </summary>
        /// <param name="income">the list for conversion</param>
        /// <returns>income report items</returns>
        public static List<FCIReport> CreateFromList(List<MLFSIncome> income)
        {
            List<FCIReport> report = new List<FCIReport>();
            report = income.GroupBy(x => new { x.Advisor }).Select(y => new FCIReport()
            {
                Advisor = y.Key.Advisor.Fullname,
                Organisation = y.Key.Advisor.Department,
                Adhoc = y.Where(a => a.IncomeType == "Ad-hoc Fee").Distinct().Sum(z => z.Amount),
                FundBased = y.Where(a => a.IncomeType == "Fund Based Commission").Distinct().Sum(z => z.Amount),
                Initial = y.Where(a => a.IncomeType == "Initial Fee" || a.IncomeType == "Initial Commission").Distinct().Sum(z => z.Amount),
                Ongoing = y.Where(a => a.IncomeType == "Ongoing Fee").Distinct().Sum(z => z.Amount),
                Renewal = y.Where(a => a.IncomeType == "Renewal Commission").Distinct().Sum(z => z.Amount),
                Other = y.Where(a => a.IncomeType != "Ad-hoc Fee" && a.IncomeType != "Fund Based Commission" && a.IncomeType != "Initial Fee" && a.IncomeType != "Initial Commission" && a.IncomeType != "Ongoing Fee" && a.IncomeType != "Renewal Commission" && !a.IgnoreFromCommission).Distinct().Sum(z => z.Amount),
                Total = y.Sum(z => z.Amount)
            }).ToList();
            return report;
        }
    }
}
