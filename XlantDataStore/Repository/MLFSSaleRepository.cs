using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XLantCore.Models;
using Microsoft.EntityFrameworkCore;

namespace XLantDataStore.Repository
{
    public class MLFSSaleRepository : IMLFSSaleRepository
    {
        private readonly XLantDbContext _db;
        private readonly IMLFSClientRepository _clientData;

        public MLFSSaleRepository(XLantDbContext db)
        {
            _db = db;
            _clientData = new MLFSClientRepository();
        }

        public async Task<List<MLFSSale>> GetDebtors(MLFSReportingPeriod period)
        {
            List<MLFSSale> debtors = await _db.MLFSSales.Where(x => (x.ReportingPeriod.Year == period.Year && x.ReportingPeriod.Month <= period.Month) || x.ReportingPeriod.Year < period.Year)
                .Include(y => y.Advisor)
                .Include(x => x.Adjustments)
                .ThenInclude(x => x.ReportingPeriod)
                .Select(d => new MLFSSale()
                {
                    Id = d.Id,
                    IOId = d.IOId,
                    IOReference = d.IOReference,
                    ReportingPeriodId = d.ReportingPeriodId,
                    ReportingPeriod = d.ReportingPeriod,
                    Organisation = d.Organisation,
                    ClientName = d.ClientName,
                    ClientId = d.ClientId,
                    JointClientId = d.JointClientId,
                    JointClientName = d.JointClientName,
                    AdvisorId = d.AdvisorId,
                    Advisor = d.Advisor,
                    ProviderName = d.ProviderName,
                    PlanType = d.PlanType,
                    IsNew = d.IsNew,
                    RelevantDate = d.RelevantDate,
                    NetAmount = d.NetAmount,
                    VAT = d.VAT,
                    Investment = d.Investment,
                    OnGoingPercentage = d.OnGoingPercentage,
                    PlanReference = d.PlanReference,
                    EstimatedOtherIncome = d.EstimatedOtherIncome,
                    Adjustments = d.Adjustments.Where(y => (y.ReportingPeriod.Year == period.Year && y.ReportingPeriod.Month <= period.Month) || y.ReportingPeriod.Year < period.Year).ToList()
                })
                .ToListAsync();
                
            //foreach (MLFSSale debtor in debtors)
            //{
            //    List<MLFSDebtorAdjustment> adjs = debtor.Adjustments.ToList();
            //    for(int i = 0; i < adjs.Count; i++)
            //    {
            //        MLFSDebtorAdjustment adj = adjs[i];
            //        if ((adj.ReportingPeriod.Year == period.Year && adj.ReportingPeriod.Month > period.Month) || adj.ReportingPeriod.Year > period.Year)
            //        {
            //            debtor.Adjustments.Remove(adj);
            //        }
            //    }
            //}
            debtors = debtors.Where(x => x.Outstanding != 0).ToList();
            return debtors;
        }
        
        public async Task<MLFSSale> GetSaleById(int saleId)
        {
            MLFSSale sale = await _db.MLFSSales.Include(x => x.ReportingPeriod).Include(y => y.Adjustments).Where(y => y.Id == saleId).FirstOrDefaultAsync();
            return sale;
        }

        public async Task<List<MLFSSale>> GetSales(MLFSReportingPeriod period)
        {
            return await _db.MLFSSales.Where(x => x.ReportingPeriodId == period.Id).Include(x => x.Adjustments).Include(x => x.Advisor).ToListAsync();
        }

        public void Update(MLFSSale sale)
        {
            _db.Entry(sale).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public async Task<string> InsertList(List<MLFSSale> sales)
        {
            foreach (MLFSSale sale in sales)
            {
                if (string.IsNullOrEmpty(sale.ProviderName))
                {
                    try
                    {
                        List<MLFSPlan> plansFromIO = await _clientData.GetClientPlans(sale.ClientId);
                        MLFSPlan selectedPlanFromIO = plansFromIO.Where(x => x.PrimaryID == sale.IOReference).FirstOrDefault();
                        if (selectedPlanFromIO != null)
                        {
                            List<MLFSFee> feesFromIO = await _clientData.GetClientFees(sale.ClientId);
                            MLFSFee selectedFeeFromIO = feesFromIO.Where(x => x.Plan.PrimaryID == selectedPlanFromIO.PrimaryID && x.IsRecurring).FirstOrDefault();
                            sale.AddPlanData(selectedPlanFromIO, selectedFeeFromIO);
                        }
                    }
                    catch
                    {
                        //No IO data found
                    }
                }
                _db.MLFSSales.Add(sale);
            }
            await _db.SaveChangesAsync();
            return "Success";
        }

        public async Task<int> Add(MLFSSale sale)
        {
            _db.MLFSSales.Add(sale);
            await _db.SaveChangesAsync();
            return sale.Id;
        }

        public async Task<List<MLFSSale>> Search(string searchTerm)
        {
            List<MLFSSale> debtors = await _db.MLFSSales.Where(x => x.ClientName.Contains(searchTerm) || x.IOReference.Contains(searchTerm) || x.PlanReference.Contains(searchTerm)).Include(y => y.Advisor).Include(x => x.Adjustments).ThenInclude(x => x.ReportingPeriod).ToListAsync();
            return debtors;
        }
    }
}
