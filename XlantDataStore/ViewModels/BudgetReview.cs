using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class BudgetReview
    {

        public BudgetReview()
        {
            Budgets = new List<MLFSBudget>();
        }

        public BudgetReview(MLFSAdvisor advisor, List<MLFSBudget> budgets, List<MLFSReportingPeriod> periods, string financialYear)
        {
            Budgets = new List<MLFSBudget>();
            AdvisorId = advisor.Id;
            Advisor = advisor;
            Year = financialYear;
            budgets = budgets.Where(x => x.ReportingPeriod.FinancialYear == financialYear && x.AdvisorId == advisor.Id).ToList();
            if (budgets.Count != 12)
            {
                periods = periods.Where(x => x.FinancialYear == financialYear).ToList();
                for (int i = 0; i < periods.Count; i++)
                {
                    if (budgets.Where(x => x.ReportingPeriodId == periods[i].Id).Count() == 0)
                    {
                        budgets.Add(new MLFSBudget() {
                            ReportingPeriodId = periods[i].Id,
                            ReportingPeriod = periods[i],
                            Budget = 0,
                            Advisor = Advisor,
                            AdvisorId = AdvisorId
                        });
                    }
                }
            }
            if (budgets != null)
            {
                Budgets = budgets;
            }
        }

        public int AdvisorId { get; set; }
        public MLFSAdvisor Advisor { get; set; }
        public string Year { get; set; }
        public List<MLFSBudget> Budgets { get; set; }
    }
}
