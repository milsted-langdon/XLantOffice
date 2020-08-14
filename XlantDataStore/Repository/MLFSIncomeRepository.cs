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
            MLFSIncome income = await _db.MLFSIncome.FindAsync(incomeId);
            return income;
        }

        public async Task<List<MLFSIncome>> GetIncome(MLFSReportingPeriod period)
        {
            return await _db.MLFSIncome.Where(x => x.ReportingPeriodId == period.Id).ToListAsync();
        }

        public async Task<List<MLFSIncome>> GetIncome(List<MLFSReportingPeriod> periods)
        {
            List<int> ids = periods.Select(x => x.Id).ToList();
            return await _db.MLFSIncome.Where(x => ids.Contains((int)x.ReportingPeriodId)).ToListAsync();
        }

        public async Task<List<MLFSIncome>> UploadIncomeForPeriod(MLFSReportingPeriod period, DataTable income)
        {
            List<MLFSIncome> returnedTrans = new List<MLFSIncome>();
            List<MLFSAdvisor> advisors = await _advisorData.GetAdvisors();
            foreach (DataRow row in income.Rows)
            {
                MLFSIncome tran = new MLFSIncome(row, advisors)
                {
                    ReportingPeriodId = period.Id,
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
    }
}
