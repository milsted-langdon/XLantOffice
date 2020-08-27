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
        private readonly IMLFSAdvisorRepository _advisorData;

        public MLFSSaleRepository(XLantDbContext db)
        {
            _db = db;
            _clientData = new MLFSClientRepository();
            _advisorData = new MLFSAdvisorRepository(db);
        }

        public async Task<List<MLFSSale>> GetDebtors()
        {
            List<MLFSSale> debtors = await _db.MLFSSales.Include(x => x.Adjustments).ToListAsync();
            debtors = debtors.Where(x => x.Outstanding > 0).ToList();
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

        public async void InsertList(List<MLFSSale> sales)
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
        }
    }
}
