using System;
using System.Collections.Generic;
using System.Text;

namespace XLantCore.Models
{
    public partial class MLFSDebtorAdjustment
    {
        /// <summary>
        /// Clones an adjustment, useful when adding a reversing entry
        /// </summary>
        /// <returns>Your cloned object</returns>
        public MLFSDebtorAdjustment Clone()
        {
            MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment
            {
                Amount = Amount,
                Debtor = Debtor,
                DebtorId = DebtorId,
                IsVariance = IsVariance,
                NotTakenUp = NotTakenUp,
                ReportingPeriod = ReportingPeriod,
                ReportingPeriodId = ReportingPeriodId
            };
            return adj;
        }
    }
}
