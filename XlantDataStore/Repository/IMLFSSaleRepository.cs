using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public interface IMLFSSaleRepository
    {
        Task<List<MLFSSale>> GetDebtors();
        Task<List<MLFSSale>> GetSales(MLFSReportingPeriod period);
        Task<MLFSSale> GetSaleById(int saleId);
        Task<List<MLFSSale>> UploadSalesForPeriod(MLFSReportingPeriod period, DataTable sales, DataTable plans);
        void Update(MLFSSale sale);

    }
}
