using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using Microsoft.Extensions.Configuration;

namespace XLantDataStore
{
    public class XLantDbContext : DbContext
    {
        public DbSet<MLFSSale> MLFSSales { get; set; }
        public DbSet<MLFSAdvisor> MLFSAdvisors { get; set; }
        public DbSet<MLFSBudget> MLFSBudgets { get; set; }
        public DbSet<MLFSCommissionRate> MLFSCommissionRates { get; set; }
        public DbSet<MLFSIncome> MLFSIncome { get; set; }
        public DbSet<MLFSReportingPeriod> MLFSReportingPeriods { get; set; }
        public DbSet<DataMap> DataMaps { get; set; }
        public DbSet<MLFSDebtorAdjustment> MLFSDebtorAdjustments { get; set; }
        public XLantDbContext(DbContextOptions<XLantDbContext> options) : base(options)
        {

        }
    }
}
