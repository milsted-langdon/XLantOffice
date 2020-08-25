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

        public async Task<List<MLFSSale>> UploadSalesForPeriod(MLFSReportingPeriod period, DataTable sales, DataTable plans)
        {
            List<MLFSSale> returnedSales = new List<MLFSSale>();
            List<MLFSAdvisor> advisors = await _advisorData.GetAdvisors();
            foreach (DataRow row in sales.Rows)
            {
                MLFSSale sale = new MLFSSale(row, advisors);
                sale.ReportingPeriodId = period.Id;
                sale.ReportingPeriod = period;
                List<DataRow> planRows = plans.AsEnumerable().Where(x => x.Field<string>("Root Sequential Ref").Contains(sale.PlanReference)).ToList();
                planRows.AddRange(plans.AsEnumerable().Where(x => x.Field<string>("Sequential Ref").Contains(sale.PlanReference)).ToList());
                DataRow selectedPlan;
                if (planRows.Count == 1)
                {
                    selectedPlan = planRows.FirstOrDefault();
                }
                else
                {
                    //match against fee ref
                    if (planRows.Where(x => x.Field<string>("Related Fee Reference").Contains(sale.IOReference)).Count() > 0)
                    {
                        selectedPlan = planRows.Where(x => x.Field<string>("Related Fee Reference").Contains(sale.IOReference)).FirstOrDefault();
                    }
                    else
                    {
                        selectedPlan = planRows.FirstOrDefault();
                    }
                }
                if (selectedPlan == null)
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
                else
                {
                    sale.AddPlanData(selectedPlan);
                }
                returnedSales.Add(sale);
                _db.MLFSSales.Add(sale);
            }
            await _db.SaveChangesAsync();
            return returnedSales;
        }
    }
}
