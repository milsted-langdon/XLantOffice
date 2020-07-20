using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using Microsoft.EntityFrameworkCore;

namespace XLantDataStore.Repository
{
    public class MLFSIncomeRepository : IMLFSIncomeRepository
    {
        private readonly XLantDbContext _db;
        
        public MLFSIncomeRepository(XLantDbContext db)
        {
            _db = db;
        }
        
        public async Task<MLFSIncome> GetIncomeById(int incomeId)
        {
            MLFSIncome income = await _db.MLFSIncome.FindAsync(incomeId);
            return income;
        }

        public async Task<List<MLFSIncome>> GetIncome(MLFSReportingPeriod period)
        {
            return await _db.MLFSIncome.Where(x => x.MLFSReportPeriodId == period.Id).ToListAsync();
        }

        public async Task<List<MLFSIncome>> UploadIncomeForPeriod(MLFSReportingPeriod period, DataTable income)
        {
            List<MLFSIncome> returnedTrans = new List<MLFSIncome>();
            foreach (DataRow row in income.Rows)
            {
                MLFSIncome tran = new MLFSIncome(row)
                {
                    MLFSReportPeriodId = period.Id,
                    ReportingPeriod = period
                };

                returnedTrans.Add(tran);
                _db.MLFSIncome.Add(tran);
            }
            await _db.SaveChangesAsync();
            return returnedTrans;
        }

        public async Task<List<MLFSIncome>> GetIncome()
        {
            return await _db.MLFSIncome.ToListAsync();
        }

    }
}
