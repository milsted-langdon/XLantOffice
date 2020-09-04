using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XLantCore.Models;
using XLantCore;
using XLantDataStore.Repository;
using Microsoft.AspNetCore.Http;
using System.IO;
using XLantDataStore.ViewModels;

namespace XLantDataStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MLFSReportsController : ControllerBase
    {
        private readonly ILogger<IntelligentOfficeController> _logger;
        private readonly IMLFSReportingPeriodRepository _periodData;
        private readonly IMLFSSaleRepository _salesData;
        private readonly IMLFSIncomeRepository _incomeData;
        private readonly IMLFSAdvisorRepository _advisorData;
        private readonly IMLFSDebtorAdjustmentRepository _adjustmentData;
        private readonly IMLFSBudgetRepository _budgetData;
        private readonly IMLFSClientRepository _clientData;

        public MLFSReportsController(ILogger<IntelligentOfficeController> logger, XLantDbContext context)
        {
            _logger = logger;
            _periodData = new MLFSReportingPeriodRepository(context);
            _salesData = new MLFSSaleRepository(context);
            _incomeData = new MLFSIncomeRepository(context);
            _advisorData = new MLFSAdvisorRepository(context);
            _adjustmentData = new MLFSDebtorAdjustmentRepository(context);
            _budgetData = new MLFSBudgetRepository(context);
            _clientData = new MLFSClientRepository();
        }

        /// <summary>
        /// Build and return the sales report for a period
        /// </summary>
        /// <returns>Sales report</returns>
        [HttpGet]
        [Route("SalesReport")]
        public async Task<List<SalesReport>> SalesReport(string periodId)
        {
            MLFSReportingPeriod period;
            List<SalesReport> report = new List<SalesReport>();
            //get period
            if (String.IsNullOrEmpty(periodId))
            {
                return null;
            }
            if (int.TryParse(periodId, out int pId))
            {
                period = await _periodData.GetPeriodById(pId);
            }
            else
            {
                return null;
            }
            
            //get advsiors
            List<MLFSAdvisor> advisors = await _advisorData.GetAdvisors();
            List<MLFSIncome> income = await _incomeData.GetIncome(period);
            List<MLFSDebtorAdjustment> adjs = await _adjustmentData.GetAdjustments(period);
            List<MLFSBudget> budgets = await _budgetData.GetBudgets(period);

            foreach (MLFSAdvisor adv in advisors)
            {
                SalesReport line = new SalesReport(income, adjs, budgets, adv, period);
                report.Add(line);
            }
            return report;
        }

        /// <summary>
        /// Returns the relevant data for a period for producing the Directors report
        /// </summary>
        /// <param name="periodId">The Id of the period we are reporting for</param>
        /// <returns>the report lines</returns>
        [HttpGet]
        [Route("DirectorsReport")]
        public async Task<List<DirectorsReport>> DirectorsReport(int periodId)
        {
            MLFSReportingPeriod period = await _periodData.GetPeriodById(periodId);
            List<MLFSSale> sales = await _salesData.GetSales(period);
            foreach (MLFSSale sale in sales)
            {
                if (!sale.InitialFeePass || !sale.OngoingFeePass)
                {
                    MLFSClient client = await _clientData.GetClient(sale.ClientId);
                    MLFSPlan plan = client.Plans.Where(x => x.PrimaryID == sale.IOReference).FirstOrDefault();
                    sale.Investment = plan.ContributionsToDate;
                    _salesData.Update(sale);
                }
            }
            List<DirectorsReport> report = ViewModels.DirectorsReport.Create(sales);
            
            return report;
        }
    }
}
