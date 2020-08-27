using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using Microsoft.EntityFrameworkCore;
using RestSharp;

namespace XLantDataStore.Repository
{
    public class MLFSIncomeRepository : IMLFSIncomeRepository
    {
        private readonly XLantDbContext _db;
        private readonly IMLFSClientRepository _clientData;
        private readonly IMLFSAdvisorRepository _advisorData;

        public MLFSIncomeRepository(XLantDbContext db)
        {
            _db = db;
            _clientData = new MLFSClientRepository();
            _advisorData = new MLFSAdvisorRepository(db);
        }
        
        public async Task<MLFSIncome> GetIncomeById(int incomeId)
        {
            MLFSIncome income = await _db.MLFSIncome.Include(x => x.ReportingPeriod).Where(y => y.Id == incomeId).FirstOrDefaultAsync();
            return income;
        }

        public async Task<List<MLFSIncome>> GetIncome(MLFSReportingPeriod period)
        {
            return await _db.MLFSIncome.Where(x => x.ReportingPeriodId == period.Id).Include(y => y.Advisor).ToListAsync();
        }

        public async Task<List<MLFSIncome>> GetIncome(List<MLFSReportingPeriod> periods)
        {
            List<int> ids = periods.Select(x => x.Id).ToList();
            return await _db.MLFSIncome.Where(x => ids.Contains((int)x.ReportingPeriodId)).ToListAsync();
        }

        public async void InsertList(List<MLFSIncome> income)
        {
            foreach (MLFSIncome i in income)
            {
                _db.MLFSIncome.Add(i);
            }
            await _db.SaveChangesAsync();
        }

        public async Task<List<MLFSIncome>> GetIncome()
        {
            return await _db.MLFSIncome.Include(x => x.ReportingPeriod).Include(x => x.Advisor).ToListAsync();
        }

        public async Task UpdateClientOnboardDate(MLFSReportingPeriod period)
        {
            List<MLFSIncome> incomeLines = await _db.MLFSIncome.Where(x => x.ReportingPeriodId == period.Id && x.ClientOnBoardDate == null).ToListAsync();
            incomeLines = incomeLines.Where(x => x.IsNewBusiness).ToList();
            if (incomeLines.Count > 0)
            {
                string[] ids = incomeLines.Select(x => x.ClientId).ToArray();
                while (ids.Length != 0)
                {
                    string[] idsForSubmission;
                    string idString = "";
                    if (ids.Length > 100)
                    {
                        idsForSubmission = ids.Take(100).ToArray();
                    }
                    else
                    {
                        idsForSubmission = ids;
                    }
                    foreach(string id in idsForSubmission)
                    {
                        if (!String.IsNullOrEmpty(id))
                        {
                            idString += id + ",";
                        }
                    }
                    idString = idString.TrimEnd(',');
                    idString = "(" + idString + ")";
                    List<MLFSClient> clients = await _clientData.GetClients(idString);
                    if (clients != null)
                    {
                        MLFSIncome.UpdateFromIO(incomeLines, clients); 
                    }
                    ids = ids.Except(idsForSubmission).ToArray();
                }
            }
        }

        public async Task<List<MLFSIncome>> PotentialDebtorMatches(MLFSSale debtor)
        {
            List<MLFSIncome> matches = new List<MLFSIncome>();

            matches.AddRange(await _db.MLFSIncome.Include(x => x.MLFSDebtorAdjustment).Where(x => x.ClientId == debtor.ClientId && x.IncomeType.Contains("Initial")).ToListAsync());
            matches.AddRange(await _db.MLFSIncome.Include(x => x.MLFSDebtorAdjustment).Where(x => x.ClientName == debtor.ClientName && x.IncomeType.Contains("Initial")).ToListAsync());
            matches.AddRange(await _db.MLFSIncome.Include(x => x.MLFSDebtorAdjustment).Where(x => x.Amount == debtor.GrossAmount && x.IncomeType.Contains("Initial")).ToListAsync());
            matches.AddRange(await _db.MLFSIncome.Include(x => x.MLFSDebtorAdjustment).Where(x => x.JointClientId == debtor.ClientId && x.IncomeType.Contains("Initial")).ToListAsync());
            //matches.AddRange(await _db.MLFSIncome.Include(x => x.MLFSDebtorAdjustment).Where(x => x.ProviderName == debtor.ProviderName && x.Amount == debtor.GrossAmount).ToListAsync());
            matches = matches.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            matches = matches.Where(x => x.MLFSDebtorAdjustment == null).ToList();
            return matches;
        }

        public async Task<List<MLFSIncome>> GetUnMatchedIncome()
        {
            List<MLFSIncome> income = await _db.MLFSIncome.Where(x => (x.IncomeType.Contains("Initial") || x.IncomeType.Contains("Ad-hoc")) && x.ClientName != "BLANK SPH").Include(y => y.MLFSDebtorAdjustment).Include(y => y.ReportingPeriod).Include(y => y.Advisor).ToListAsync();
            return income.Where(x => x.MLFSDebtorAdjustment == null).ToList();
        }
    }
}
