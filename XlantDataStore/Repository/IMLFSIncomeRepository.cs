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
        Task<List<MLFSIncome>> GetIncome();
        Task<MLFSIncome> GetIncomeById(int incomeId);
        Task<List<MLFSIncome>> UploadIncomeForPeriod(MLFSReportingPeriod period, DataTable income);

    }
}
