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

        public async Task<int> InsertPeriod(MLFSReportingPeriod period)
        {
            _db.MLFSReportingPeriods.Add(period);
            await _db.SaveChangesAsync();
            return period.Id;
        }

        public void Update(MLFSReportingPeriod period)
        {
            _db.Entry(period).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            MLFSReportingPeriod period = _db.MLFSReportingPeriods.Find(id);
            _db.MLFSReportingPeriods.Remove(period);
            _db.SaveChanges();
        }

    }
}

