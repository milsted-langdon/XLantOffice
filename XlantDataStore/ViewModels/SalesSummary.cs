using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class SalesSummary
    {
        public SalesSummary()
        {

        }
        public SalesSummary(MLFSAdvisor adv, List<SalesReport> lines)
        {
            Advisor = adv.Fullname;
            AdvisorId = adv.Id;
            Organisation = adv.Department;
            Q1Budget = lines.Where(x => x.Quarter == 1).Sum(y => y.Budget);
            Q1Actual = lines.Where(x => x.Quarter == 1).Sum(y => y.Total);
            if (Q1Budget != 0)
            {
                Q1Variance = (int)decimal.Round(Q1Actual / Q1Budget * 100, 0); 
            }
            Q2Budget = lines.Where(x => x.Quarter == 2).Sum(y => y.Budget);
            Q2Actual = lines.Where(x => x.Quarter == 2).Sum(y => y.Total);
            if (Q2Budget != 0)
            {
                Q2Variance = (int)decimal.Round(Q2Actual / Q2Budget * 100, 0); 
            }
            Q3Budget = lines.Where(x => x.Quarter == 3).Sum(y => y.Budget);
            Q3Actual = lines.Where(x => x.Quarter == 3).Sum(y => y.Total);
            if (Q3Budget != 0)
            {
                Q3Variance = (int)decimal.Round(Q3Actual / Q3Budget * 100, 0); 
            }
            Q4Budget = lines.Where(x => x.Quarter == 4).Sum(y => y.Budget);
            Q4Actual = lines.Where(x => x.Quarter == 4).Sum(y => y.Total);
            if (Q4Budget != 0)
            {
                Q4Variance = (int)decimal.Round(Q4Actual / Q4Budget * 100, 0); 
            }
            YearToDateBudget = Q1Budget + Q2Budget + Q3Budget + Q4Budget;
            YearToDateActual = Q1Actual + Q2Actual + Q3Actual + Q4Actual;
            if (YearToDateBudget != 0)
            {
                YearToDateVariance = (int)decimal.Round(YearToDateActual / YearToDateBudget * 100, 0); 
            }
        }

        public string Advisor { get; set; }
        public int AdvisorId { get; set; }
        public string Organisation { get; set; }
        [Display(Name="Q1 Budget")]
        public decimal Q1Budget { get; set; }
        [Display(Name = "Q1 Actual")]
        public decimal Q1Actual { get; set; }
        [Display(Name = "Variance")]
        public int Q1Variance { get; set; }
        [Display(Name = "Q2 Budget")]
        public decimal Q2Budget { get; set; }
        [Display(Name = "Q2 Actual")]
        public decimal Q2Actual { get; set; }
        [Display(Name = "Variance")]
        public int Q2Variance { get; set; }
        [Display(Name = "Q3 Budget")]
        public decimal Q3Budget { get; set; }
        [Display(Name = "Q3 Actual")]
        public decimal Q3Actual { get; set; }
        [Display(Name = "Variance")]
        public int Q3Variance { get; set; }
        [Display(Name = "Q4 Budget")]
        public decimal Q4Budget { get; set; }
        [Display(Name = "Q4 Actual")]
        public decimal Q4Actual { get; set; }
        [Display(Name = "Variance")]
        public int Q4Variance { get; set; }
        [Display(Name = "Year To Date Budget")]
        public decimal YearToDateBudget { get; set; }
        [Display(Name = "Year To Date Actual")]
        public decimal YearToDateActual { get; set; }
        [Display(Name = "Variance")]
        public int YearToDateVariance { get; set; }


        public static List<SalesSummary> CreateFromSalesReport(List<SalesReport> sales, List<MLFSAdvisor> advisors)
        {
            List<SalesSummary> report = new List<SalesSummary>();
            foreach (MLFSAdvisor adv in advisors)
            {
                List<SalesReport> lines = sales.Where(x => x.AdvisorId == adv.Id).ToList();
                SalesSummary sum = new SalesSummary(adv, lines);
                report.Add(sum);
            }
                        
            return report;
        }
    }
}
