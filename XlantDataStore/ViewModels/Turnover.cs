using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.ViewModels
{
    public class Turnover
    {
        public Turnover()
        {

        }

        public Turnover(MLFSAdvisor adv, List<MLFSSale> openingDebtors, List<MLFSSale> closingDebtors, List<MLFSIncome> receipts, List<MLFSSale> newBusiness, List<MLFSDebtorAdjustment> adjs)
        {
            List<MLFSDebtorAdjustment> relevantAdjustments = adjs.Where(x => x.Debtor.AdvisorId == adv.Id).ToList();
            Advisor = adv.Fullname;
            Organisation = adv.Department;
            openingDebtors = openingDebtors.Where(x => x.AdvisorId == adv.Id).ToList();
            OpeningDebtor = openingDebtors.Sum(y => y.NetOutstanding);
            closingDebtors = closingDebtors.Where(x => x.AdvisorId == adv.Id).ToList();
            ClosingDebtor = closingDebtors.Sum(y => y.NetOutstanding);
            CashReceipts = relevantAdjustments.Where(x => x.ReceiptId != null).Sum(y => y.Amount);
            NotTakenUp = relevantAdjustments.Where(x => x.NotTakenUp).Sum(y => y.Amount);
            Adjustments = relevantAdjustments.Where(x => x.ReceiptId == null && !x.NotTakenUp && !x.IsVariance).Sum(y => y.Amount);
            Variance = relevantAdjustments.Where(x => x.IsVariance).Sum(y => y.Amount);
            NewBusiness = newBusiness.Where(x => x.AdvisorId == adv.Id).Sum(y => y.NetAmount);
            DebtorBalancing = OpeningDebtor + NewBusiness + CashReceipts + NotTakenUp + Adjustments + Variance - ClosingDebtor;
            ChangeInDebtors = ClosingDebtor - OpeningDebtor;
            RecurringIncome = receipts.Where(x => x.AdvisorId == adv.Id).Sum(y => y.Amount) - CashReceipts;
            TurnoverTotal =  RecurringIncome + ChangeInDebtors;
        }

        public string Advisor { get; set; }
        public string Organisation { get; set; }
        [Display(Name = "Opening Period")]
        public string Period { get; set; }
        public int PeriodId { get; set; }
        [Display(Name = "Cash Receipt")]
        public decimal CashReceipts { get; set; }
        [Display(Name = "New Business")]
        public decimal NewBusiness { get; set; }
        [Display(Name = "Opening Debtors (Net)")]
        public decimal OpeningDebtor { get; set; }
        [Display(Name = "Closing Debtor (Net)")]
        public decimal ClosingDebtor { get; set; }
        public decimal Adjustments { get; set; }
        public decimal Variance { get; set; }
        [Display(Name = "Not Taken Up")]
        public decimal NotTakenUp { get; set; }
        [Display(Name = "Debtor Balancing")]
        public decimal DebtorBalancing { get; set; }
        [Display(Name = "Change in Debtors")]
        public decimal ChangeInDebtors { get; set; }
        [Display(Name = "Recurring Income")]
        public decimal RecurringIncome { get; set; }
        [Display(Name = "Turnover Total")]
        public decimal TurnoverTotal { get; set; }

        public static List<Turnover> CreateList(List<MLFSSale> openingDebtors, List<MLFSSale> closingDebtors, List<MLFSIncome> receipts, List<MLFSSale> newBusiness, List<MLFSDebtorAdjustment> adjs)
        {
            List<Turnover> turnovers = new List<Turnover>();
            List<MLFSAdvisor> advisors = new List<MLFSAdvisor>();
            advisors.AddRange(openingDebtors.Select(x => x.Advisor).Distinct());
            advisors.AddRange(closingDebtors.Select(x => x.Advisor).Distinct());
            advisors.AddRange(receipts.Select(x => x.Advisor).Distinct());
            advisors = advisors.Distinct().ToList();

            foreach(MLFSAdvisor adv in advisors)
            {
                Turnover t = new Turnover(adv, openingDebtors, closingDebtors, receipts, newBusiness, adjs);
                turnovers.Add(t);
            }

            return turnovers;
        }
    }
}
