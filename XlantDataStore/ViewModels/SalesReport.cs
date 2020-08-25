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
        public SalesReport(List<MLFSSale> sales, List<MLFSIncome> income, List<MLFSDebtorAdjustment> adjustments, List<MLFSBudget> budgets, MLFSAdvisor advisor, MLFSReportingPeriod period)
        {
            List<MLFSIncome> relevantIncome = income.Where(x => x.AdvisorId == advisor.Id && x.ReportingPeriodId == period.Id).ToList();
            List<MLFSSale> relevantSales = sales.Where(x => x.ReportingPeriodId == period.Id && x.AdvisorId == advisor.Id).ToList();
            List<MLFSDebtorAdjustment> relevantAdjustments = adjustments.Where(x => x.ReportingPeriodId == period.Id && x.Debtor.AdvisorId == advisor.Id).ToList();
            Period = period.Description;
            Advisor = advisor.Fullname;
            MLFSBudget budget = budgets.Where(x => x.AdvisorId == advisor.Id && x.ReportingPeriodId == period.Id).FirstOrDefault();
            if (budget != null)
            {
                Budget = budget.Budget; 
            }
            else
            {
                Budget = 0;
            }
            New_Business = relevantSales.Sum(y => y.NetAmount);
            Renewals = relevantIncome.Where(x => !x.IsNewBusiness).Sum(y => y.Amount);
            Clawback = relevantIncome.Where(x => x.IsClawBack).Sum(y => y.Amount);
            NotTakenUp = relevantAdjustments.Where(x => x.NotTakenUp).Sum(y => y.Amount);
            Adjustment = relevantIncome.Where(x => x.IsAdjustment).Sum(y => y.Amount);
            Debtors_Adjustment = relevantAdjustments.Where(x => x.ReceiptId == null && !x.NotTakenUp && !x.IsVariance).Sum(y => y.Amount);
            Debtors_Variance = relevantAdjustments.Where(x => x.IsVariance).Sum(y => y.Amount);
            Total = New_Business + Adjustment + Renewals + Clawback + NotTakenUp + Debtors_Adjustment + Debtors_Variance;
        }

        public string Period { get; set; }
        public string Advisor { get; set; }
        public decimal Budget { get; set; }
        [Display(Name="New Business")]
        public decimal New_Business { get; set; }
        public decimal Renewals { get; set; }
        public decimal Clawback { get; set; }
        [Display(Name = "Not Taken Up")]
        public decimal NotTakenUp { get; set; }
        public decimal Adjustment { get; set; }
        [Display(Name = "Debtor Variance")]
        public decimal Debtors_Variance { get; set; }
        [Display(Name = "Debtor Adjustments")]
        public decimal Debtors_Adjustment { get; set; }
        public decimal Total { get; set; }

    }
}
