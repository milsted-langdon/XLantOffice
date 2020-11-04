using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class SalesReport
    {
        public SalesReport()
        {

        }
        public SalesReport(List<MLFSIncome> income, List<MLFSSale> sales, List<MLFSDebtorAdjustment> adjustments, List<MLFSBudget> budgets, MLFSAdvisor advisor, MLFSReportingPeriod period)
        {
            List<MLFSIncome> relevantIncome = income.Where(x => x.AdvisorId == advisor.Id && x.ReportingPeriodId == period.Id).ToList();
            List<MLFSDebtorAdjustment> relevantAdjustments = adjustments.Where(x => x.ReportingPeriodId == period.Id && x.Debtor.AdvisorId == advisor.Id).ToList();
            Organisation = advisor.Department;
            Period = period.Description;
            PeriodId = period.Id;
            decimal q = period.ReportOrder / 3;
            Quarter = period.Quarter;
            Advisor = advisor.Fullname;
            AdvisorId= advisor.Id;
            MLFSBudget budget = budgets.Where(x => x.AdvisorId == advisor.Id && x.ReportingPeriodId == period.Id).FirstOrDefault();
            if (budget != null)
            {
                Budget = budget.Budget;
            }
            else
            {
                Budget = 0;
            }
            New_Business = sales.Where(x => x.AdvisorId == advisor.Id).Sum(y => y.NetAmount);
            Renewals = relevantIncome.Where(x => !x.IsNewBusiness && !x.IsClawBack).Sum(y => y.Amount);
            Unallocated = relevantIncome.Where(x => x.IsNewBusiness && !x.IsClawBack && x.MLFSDebtorAdjustment == null).Sum(y => y.Amount);
            Clawback = relevantIncome.Where(x => x.IsClawBack).Sum(y => y.Amount);
            NotTakenUp = relevantAdjustments.Where(x => x.NotTakenUp).Sum(y => y.Amount);
            Adjustment = relevantIncome.Where(x => x.IsAdjustment).Sum(y => y.Amount);
            Debtors_Adjustment = relevantAdjustments.Where(x => x.ReceiptId == null && !x.NotTakenUp && !x.IsVariance).Sum(y => y.Amount);
            Debtors_Variance = relevantAdjustments.Where(x => x.IsVariance).Sum(y => y.Amount);
            Total = New_Business + Adjustment + Renewals + Unallocated + Clawback + NotTakenUp + Debtors_Adjustment + Debtors_Variance;
        }

        public string Period { get; set; }
        public int PeriodId { get; set; }
        public int Quarter { get; set; }
        public string Advisor { get; set; }
        public int AdvisorId { get; set; }
        public string Organisation { get; set; }
        public decimal Budget { get; set; }
        [Display(Name = "New Business")]
        public decimal New_Business { get; set; }
        public decimal Renewals { get; set; }
        [Display(Name = "Unallocated New Business")]
        public decimal Unallocated { get; set; }
        public decimal Clawback { get; set; }
        [Display(Name = "Not Taken Up")]
        public decimal NotTakenUp { get; set; }
        public decimal Adjustment { get; set; }
        [Display(Name = "Debtor Variance")]
        public decimal Debtors_Variance { get; set; }
        [Display(Name = "Debtor Adjustments")]
        public decimal Debtors_Adjustment { get; set; }
        public decimal Total { get; set; }


        public static List<SalesReport> CreateByCampaign(List<MLFSIncome> income, MLFSReportingPeriod period)
        {
            List<SalesReport> salesReports = income.Where(w => w.ReportingPeriodId == period.Id).GroupBy(x => x.Campaign).Select(y => new SalesReport()
            {
                Period = period.Description,
                Advisor = y.Key,
                Budget = 0,
                New_Business = y.Where(a => a.IsNewBusiness && a.MLFSDebtorAdjustment != null).Sum(b => b.Amount),
                Renewals = y.Where(a => !a.IsNewBusiness).Sum(b => b.Amount),
                Unallocated = y.Where(a => a.IsNewBusiness && a.MLFSDebtorAdjustment == null).Sum(b => b.Amount),
                Clawback = y.Where(a => a.IsClawBack).Sum(b => b.Amount),
                NotTakenUp = 0,
                Adjustment = 0,
                Debtors_Adjustment = 0,
                Debtors_Variance = 0
            }).ToList();

            return salesReports;
        }
    }
}
