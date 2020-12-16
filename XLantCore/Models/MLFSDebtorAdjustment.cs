using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSDebtorAdjustment
    {
        public MLFSDebtorAdjustment()
        {

        }

        /// <summary>
        /// matches the income to the sale/debtor
        /// </summary>
        /// <param name="sale"></param>
        /// <param name="income"></param>
        public MLFSDebtorAdjustment(MLFSSale sale, MLFSIncome income)
        {
            MLFSReportingPeriod period = income.ReportingPeriod;
            ReportingPeriodId = period.Id;
            ReportingPeriod = period;
            DebtorId = (int)sale.Id;
            Debtor = sale;
            ReceiptId = income.Id;
            Amount = income.Amount *-1;
            IsVariance = false;
            NotTakenUp = false;
        }

        public int Id { get; set; }
        [Display(Name = "Reporting Period")]
        public int ReportingPeriodId { get; set; }
        [Display(Name = "Debtor")]
        public int DebtorId { get; set; }
        [Display(Name = "Income")]
        public int? ReceiptId { get; set; }
        public decimal Amount { get; set; }
        [Display(Name = "Variance")]
        public bool IsVariance { get; set; }
        [Display(Name = "NTU")]
        public bool NotTakenUp { get; set; }

        public virtual MLFSReportingPeriod ReportingPeriod { get; set; }
        public virtual MLFSSale Debtor { get; set; }
        public virtual MLFSIncome Receipt { get; set; }
    }
}
