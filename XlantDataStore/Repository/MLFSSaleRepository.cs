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
        private readonly IMLFSClientRepository _clientRepository;

        public MLFSSaleRepository(XLantDbContext db)
        {
            _db = db;
            _clientRepository = new MLFSClientRepository();
        }
        
        public async Task<MLFSSale> GetSaleById(int saleId)
        {
            MLFSSale sale = await _db.MLFSSales.FindAsync(saleId);
            return sale;
        }

        public async Task<IEnumerable<MLFSSale>> GetSales(MLFSReportingPeriod period)
        {
            return await _db.MLFSSales.Where(x => x.ReportPeriodId == period.Id).ToListAsync();
        }

        public async Task<List<MLFSSale>> UploadSalesForPeriod(MLFSReportingPeriod period, DataTable sales, DataTable plans)
        {
            List<MLFSSale> returnedSales = new List<MLFSSale>();
            foreach (DataRow row in sales.Rows)
            {
                MLFSSale sale = new MLFSSale(row);
                sale.ReportPeriodId = period.Id;
                sale.ReportingPeriod = period;
                List<DataRow> planRows = plans.AsEnumerable().Where(x => x.Field<string>("Root Sequential Ref") == sale.PlanReference).ToList();
                planRows.AddRange(plans.AsEnumerable().Where(x => x.Field<string>("Sequential Ref") == sale.PlanReference).ToList());
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
                        List<Plan> plansFromIO = await _clientRepository.GetClientPlans(sale.ClientId);
                        Plan selectedPlanFromIO = plansFromIO.Where(x => x.Reference == sale.IOReference).FirstOrDefault();

                        if (selectedPlanFromIO != null)
                        {
                            List<Fee> feesFromIO = await _clientRepository.GetClientFees(sale.ClientId);
                            Fee selectedFeeFromIO = feesFromIO.Where(x => x.Plan.PrimaryID == selectedPlanFromIO.PrimaryID && x.IsRecurring).FirstOrDefault();
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
