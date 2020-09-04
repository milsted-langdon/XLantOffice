using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XLantCore;
using Microsoft.EntityFrameworkCore;

namespace XLantDataStore.Repository
{
    public class MLFSDebtorAdjustmentRepository : IMLFSDebtorAdjustmentRepository
    {
        private readonly XLantDbContext _context;
        public MLFSDebtorAdjustmentRepository(XLantDbContext context)
        {
            _context = context;
        }

        public void Delete(int adjustmentId)
        {
            MLFSDebtorAdjustment adj = _context.MLFSDebtorAdjustments.Find(adjustmentId);
            if (adj != null)
            {
                _context.MLFSDebtorAdjustments.Remove(adj);
                _context.SaveChanges();
            }
        }

        public async Task<MLFSDebtorAdjustment> GetAdjustmentById(int adjustmentId)
        {
            MLFSDebtorAdjustment adj = await _context.MLFSDebtorAdjustments.FindAsync(adjustmentId);
            return adj;
        }

        public async Task<List<MLFSDebtorAdjustment>> GetAdjustments(int debtorId)
        {
            List<MLFSDebtorAdjustment> adjs = await _context.MLFSDebtorAdjustments.Where(x => x.DebtorId == debtorId).Include(y => y.Debtor).Include(z => z.Receipt).Include(a => a.ReportingPeriod).ToListAsync();
            return adjs;
        }

        public async Task<List<MLFSDebtorAdjustment>> GetAdjustments(MLFSReportingPeriod period)
        {
            List<MLFSDebtorAdjustment> adjs = await _context.MLFSDebtorAdjustments.Include(y => y.Debtor).Include(y => y.Receipt).Include(y => y.Debtor.Advisor).Where(x => x.ReportingPeriodId == period.Id).ToListAsync();
            return adjs;
        }

        public void Insert(MLFSDebtorAdjustment adjustment)
        {
            _context.MLFSDebtorAdjustments.Add(adjustment);
            _context.SaveChanges();
        }

        public void InsertList(List<MLFSDebtorAdjustment> adjs)
        {
            for (int i = 0; i < adjs.Count; i++)
            {
                _context.MLFSDebtorAdjustments.Add(adjs[i]);
            }
            _context.SaveChanges();
        }

        public void Update(MLFSDebtorAdjustment adjustment)
        {
            _context.Entry(adjustment).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }

}
