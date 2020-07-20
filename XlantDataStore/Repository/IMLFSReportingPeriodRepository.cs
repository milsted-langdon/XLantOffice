using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IMLFSReportingPeriodRepository
    {
        Task<List<MLFSReportingPeriod>> GetPeriods();
        Task<MLFSReportingPeriod> GetPeriodById(int periodId);
        Task<int> InsertPeriod(MLFSReportingPeriod period);
        void Update(MLFSReportingPeriod period);
        void Delete(int Id);
    }
}
