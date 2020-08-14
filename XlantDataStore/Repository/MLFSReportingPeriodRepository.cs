using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<List<MLFSReportingPeriod>> GetLast12Months(MLFSReportingPeriod period)
        {
            return await _db.MLFSReportingPeriods.Where(x => x.FinancialYear == period.FinancialYear || (x.FinancialYear == period.PriorYear && x.ReportOrder > period.ReportOrder)).ToListAsync();
        }

        public async Task<List<MLFSReportingPeriod>> GetFinancialYear(MLFSReportingPeriod period)
        {
            return await _db.MLFSReportingPeriods.Where(x => x.FinancialYear == period.FinancialYear).ToListAsync();
        }

        public async Task<MLFSReportingPeriod> GetPeriodById(int periodId)
        {
            MLFSReportingPeriod period = await _db.MLFSReportingPeriods.Where(y => y.Id == periodId).FirstOrDefaultAsync();
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

        public async Task<SelectList> SelectList(int? periodId=null)
        {
            List<MLFSReportingPeriod> periods = await GetPeriods();
            if(periodId == null)
            {
                MLFSReportingPeriod period = await GetCurrent();
                periodId = period.Id;
            }
            SelectList sList = new SelectList(periods.OrderByDescending(x => x.Year).ThenByDescending(y => y.ReportOrder), "Id", "Description", periodId);
            return sList;
        }

        public async Task<MLFSReportingPeriod> GetCurrent()
        {
            MLFSReportingPeriod period = await _db.MLFSReportingPeriods.Where(x => x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month).FirstOrDefaultAsync();
            if (period == null)
            {
                DateTime lastMonth = DateTime.Now.AddMonths(-1);
                period = await _db.MLFSReportingPeriods.Where(x => x.Year == lastMonth.Year && x.Month == lastMonth.Month).FirstOrDefaultAsync();
            }
            if (period == null)
            {
                period = await _db.MLFSReportingPeriods.OrderByDescending(x => x.Year).ThenByDescending(y => y.ReportOrder).FirstOrDefaultAsync();
            }
            return period;
        }
    }
}

