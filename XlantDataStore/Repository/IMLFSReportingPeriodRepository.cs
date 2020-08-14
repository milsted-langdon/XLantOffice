using Microsoft.AspNetCore.Mvc.Rendering;
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
        Task<List<MLFSReportingPeriod>> GetLast12Months(MLFSReportingPeriod period);
        Task<List<MLFSReportingPeriod>> GetFinancialYear(MLFSReportingPeriod period);
        Task<MLFSReportingPeriod> GetPeriodById(int periodId);
        Task<int> InsertPeriod(MLFSReportingPeriod period);
        void Update(MLFSReportingPeriod period);
        void Delete(int Id);
        Task<SelectList> SelectList(int? periodId=null);
        Task<MLFSReportingPeriod> GetCurrent();
    }
}
