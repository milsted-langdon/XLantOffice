using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IMLFSIncomeRepository
    {
        Task<List<MLFSIncome>> GetIncome(MLFSReportingPeriod period);
        Task<List<MLFSIncome>> GetIncome(List<MLFSReportingPeriod> periods);
        Task<List<MLFSIncome>> GetIncome();
        Task<MLFSIncome> GetIncomeById(int incomeId);
        Task UpdateClientOnboardDate(MLFSReportingPeriod period);
        Task<List<MLFSIncome>> PotentialDebtorMatches(MLFSSale debtor);
        Task<List<MLFSIncome>> GetUnMatchedIncome();
        Task<string> InsertList(List<MLFSIncome> income);
    }
}
