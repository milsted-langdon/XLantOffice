using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;

namespace XLantDataStore.Repository
{
    public class MLFSBudgetRepository : IMLFSBudgetRepository
    {
        private readonly XLantDbContext _context;
        public MLFSBudgetRepository(XLantDbContext context)
        {
            _context = context;
        }

        public void Delete(int budgetId)
        {
            MLFSBudget budget = _context.MLFSBudgets.Find(budgetId);
            _context.Entry(budget).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
        }

        public async Task<List<MLFSBudget>> GetBudgets(MLFSReportingPeriod period)
        {
            List<MLFSBudget> budgets = await _context.MLFSBudgets.Where(x => x.ReportingPeriodId == period.Id).ToListAsync();
            return budgets;
        }

        public async Task<List<MLFSBudget>> GetBudgets(List<MLFSReportingPeriod> periods)
        {
            List<int> ids = periods.Select(x => x.Id).ToList();
            List<MLFSBudget> budgets = await _context.MLFSBudgets.Where(x => ids.Contains((int)x.ReportingPeriodId)).ToListAsync();
            return budgets;
        }

        public async Task<MLFSBudget> GetById(string id)
        {
            MLFSBudget budget = await _context.MLFSBudgets.FindAsync(id);
            return budget;
        }

        public void Insert(MLFSBudget budget)
        {
            _context.MLFSBudgets.Add(budget);
            _context.SaveChanges();
        }

        public void Update(MLFSBudget budget)
        {
            _context.Entry(budget).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
