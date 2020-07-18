using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using Microsoft.EntityFrameworkCore;

namespace XLantDataStore.Repository
{
    public class MLFSReportingPeriodRepository : IMLFSReportingPeriodRepository
    {
        private readonly XLantDbContext _db;

        public MLFSReportingPeriodRepository(XLantDbContext db)
        {
            this._db = db;
        }

        public async Task<List<MLFSReportingPeriod>> GetPeriods()
        {
            return await _db.MLFSReportingPeriods.ToListAsync();
        }

        public async Task<MLFSReportingPeriod> GetPeriodById(int periodId)
        {
            MLFSReportingPeriod period = await _db.MLFSReportingPeriods.FindAsync(periodId);
            return period;
        }

        public void InsertPeriod(MLFSReportingPeriod period)
        {
            _db.MLFSReportingPeriods.Add(period);
            _db.SaveChangesAsync();
        }

    }
}

