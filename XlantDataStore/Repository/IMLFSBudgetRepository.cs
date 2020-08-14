using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IMLFSBudgetRepository
    {
        Task<MLFSBudget> GetById(string id);
        Task<List<MLFSBudget>> GetBudgets(MLFSReportingPeriod period);
        Task<List<MLFSBudget>> GetBudgets(List<MLFSReportingPeriod> periods);
        void Update(MLFSBudget budget);
        void Delete(int budgetId);
        void Insert(MLFSBudget budget);
    }
}
