using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class IncomeReport
    {
        public IncomeReport()
        {

        }

        public IncomeReport(MLFSIncome income)
        {
            Period = income.ReportingPeriod.Description;
            PeriodId = income.ReportingPeriod.Id;
            Organisation = income.Organisation;
            if (income.Advisor != null)
            {
                Advisor = income.Advisor.Fullname;
                AdvisorId = income.AdvisorId;
            }
            else
            {
                Advisor = "Unknown";
                AdvisorId = 0;
            }
            if (income.IsNewBusiness)
            {
                New_Amount = income.Amount;
            }
            else
            {
                Existing_Amount = income.Amount;
            }
            Amount = income.Amount;
            Budget = 0;
        }

        public string Period { get; set; }
        public int PeriodId { get; set; }
        public string Organisation { get; set; }
        public string Advisor { get; set; }
        public int AdvisorId { get; set; }
        public decimal Budget { get; set; }
        public decimal New_Amount { get; set; }
        public decimal Existing_Amount { get; set; }
        public decimal Amount { get; set; }

        /// <summary>
        /// Creates a list of income entries from MLFSIncome objects
        /// </summary>
        /// <param name="income">the list for conversion</param>
        /// <returns>income report items</returns>
        public static List<IncomeReport> CreateFromList(List<MLFSIncome> income)
        {
            List<IncomeReport> report = new List<IncomeReport>();
            foreach (MLFSIncome i in income)
            {
                IncomeReport rep = new IncomeReport(i);
                report.Add(rep);
            }
            return report;
        }

        /// <summary>
        /// Pivots the income report by grouping by advisor
        /// </summary>
        /// <param name="periods">the periods on which we are reporting</param>
        /// <returns>Income Report entries</returns>
        public static List<IncomeReport> CreateReportByAdvisor(List<MLFSReportingPeriod> periods)
        {
            List<MLFSIncome> incomeLines = periods.SelectMany(x => x.Receipts).ToList();
            List<MLFSBudget> budgets = periods.SelectMany(x => x.Budgets).ToList();
            List<IncomeReport> report = CreateFromList(incomeLines);

            report = report.GroupBy(x => new { x.Period, x.PeriodId, x.Advisor, x.AdvisorId }).Select(y => new IncomeReport()
            {
                Advisor = y.Key.Advisor,
                AdvisorId = y.Key.AdvisorId,
                Organisation = "",
                Amount = y.Sum(z => z.Amount),
                Existing_Amount = y.Sum(z => z.Existing_Amount),
                New_Amount = y.Sum(z => z.New_Amount),
                Period = y.Key.Period,
                PeriodId = y.Key.PeriodId
            }).ToList();
            foreach (IncomeReport r in report)
            {
                if (budgets.Where(x => x.AdvisorId == r.AdvisorId && x.ReportingPeriodId == r.PeriodId).ToList().Count > 0)
                {
                    r.Budget = Tools.HandleNull(budgets.Where(x => x.AdvisorId == r.AdvisorId && x.ReportingPeriodId == r.PeriodId).FirstOrDefault().Budget);
                }
                else
                {
                    r.Budget = 0;
                }
            }
            AddZeroEntries(periods, report, true);
            return report;
        }

        /// <summary>
        /// Pivots the income report by grouping by organisation
        /// </summary>
        /// <param name="periods">the periods on which we are reporting</param>
        /// <returns>Income Report entries</returns>
        public static List<IncomeReport> CreateReportByOrganisation(List<MLFSReportingPeriod> periods)
        {
            List<MLFSIncome> incomeLines = periods.SelectMany(x => x.Receipts).ToList();
            List<MLFSBudget> budgets = periods.SelectMany(x => x.Budgets).ToList();
            List<IncomeReport> report = CreateFromList(incomeLines);

            report = report.GroupBy(x => new { x.Period, x.PeriodId, x.Organisation }).Select(y => new IncomeReport()
            {
                Advisor = "",
                AdvisorId = 0,
                Organisation = y.Key.Organisation,
                Amount = y.Sum(z => z.Amount),
                Existing_Amount = y.Sum(z => z.Existing_Amount),
                New_Amount = y.Sum(z => z.New_Amount),
                Period = y.Key.Period,
                PeriodId = y.Key.PeriodId
            }).ToList();
            foreach (IncomeReport r in report)
            {
                if (budgets.Where(x => x.Advisor.Department == r.Organisation && x.ReportingPeriodId == r.PeriodId).ToList().Count > 0)
                {
                    r.Budget = Tools.HandleNull(budgets.Where(x => x.Advisor.Department == r.Organisation && x.ReportingPeriodId == r.PeriodId).FirstOrDefault().Budget);
                }
                else
                {
                    r.Budget = 0;
                }
            }
            AddZeroEntries(periods, report, false);
            return report;
        }

        /// <summary>
        /// Add the 0 entries to the report where the period and advisor/org exists but there happen to be no entries
        /// </summary>
        /// <param name="periods">the periods we are reporting on</param>
        /// <param name="reportLines">The lines we already have</param>
        /// <param name="byAdvisor">if true then pivoted by advisor otherwise, organisation is assumed</param>
        private static void AddZeroEntries(List<MLFSReportingPeriod> periods, List<IncomeReport> reportLines, bool byAdvisor)
        {

            foreach (MLFSReportingPeriod period in periods)
            {
                if (byAdvisor)
                {
                    List<MLFSAdvisor> advisors = periods.SelectMany(x => x.Receipts).Select(y => y.Advisor).Distinct().ToList();
                    foreach (MLFSAdvisor advisor in advisors)
                    {
                        List<IncomeReport> entries = reportLines.Where(x => x.AdvisorId == advisor.Id && x.PeriodId == period.Id).ToList();
                        if (entries == null || entries.Count == 0)
                        {
                            IncomeReport entry = new IncomeReport()
                            {
                                Period = period.Description,
                                PeriodId = period.Id,
                                Advisor = advisor.Fullname,
                                AdvisorId = advisor.Id,
                                Organisation = "",
                                Amount = 0,
                                New_Amount = 0,
                                Existing_Amount = 0
                            };
                            if (period.Budgets.Where(x => x.AdvisorId == advisor.Id).ToList().Count > 0)
                            {
                                entry.Budget = Tools.HandleNull(period.Budgets.Where(x => x.AdvisorId == advisor.Id).FirstOrDefault().Budget); 
                            }
                            else
                            {
                                entry.Budget = 0;
                            }
                            reportLines.Add(entry);
                        }
                    }
                }
                else
                {
                    string[] orgs = periods.SelectMany(x => x.Receipts).Select(y => y.Organisation).Distinct().ToArray();
                    foreach (string org in orgs)
                    {
                        List<IncomeReport> entries = reportLines.Where(x => x.Organisation == org && x.PeriodId == period.Id).ToList();
                        if (entries == null || entries.Count == 0)
                        {
                            IncomeReport entry = new IncomeReport()
                            {
                                Period = period.Description,
                                PeriodId = period.Id,
                                Advisor = "",
                                AdvisorId = 0,
                                Organisation = org,
                                Amount = 0,
                                New_Amount = 0,
                                Existing_Amount = 0
                            };
                            if (period.Budgets.Where(x => x.Advisor.Department == org).ToList().Count > 0)
                            {
                                entry.Budget = Tools.HandleNull(period.Budgets.Where(x => x.Advisor.Department == org).FirstOrDefault().Budget);
                            }
                            else
                            {
                                entry.Budget = 0;
                            }
                            reportLines.Add(entry);
                        }
                    }
                }
            }
        }
    }
}
