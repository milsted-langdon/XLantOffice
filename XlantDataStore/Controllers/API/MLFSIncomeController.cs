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
    public class MLFSIncomeController : ControllerBase
    {
        private readonly ILogger<IntelligentOfficeController> _logger;
        private readonly IMLFSReportingPeriodRepository _periodData;
        private readonly IMLFSIncomeRepository _incomeData;
        private readonly IMLFSBudgetRepository _budgetData;

        public MLFSIncomeController(ILogger<IntelligentOfficeController> logger, XLantDbContext context)
        {
            _logger = logger;
            _periodData = new MLFSReportingPeriodRepository(context);
            _incomeData = new MLFSIncomeRepository(context);
            _budgetData = new MLFSBudgetRepository(context);
        }

        /// <summary>
        /// Returns the income for a period, either a single period or the current FY or last 12 months.  If both are true currentFY takes precedence
        /// </summary>
        /// <param name="currentMonthId">The Id of the month we are reporting on</param>
        /// <param name="currentFY">if true returns the income in the periods up to and including the one we are reporting</param>
        /// <param name="last12periods">if true returns the income in every period over the last twelve.</param>
        /// <returns>a List of income</returns>
        [HttpGet]
        [Route("GetIncome")]
        public async Task<List<MLFSIncomeReport>> GetIncome(int currentMonthId, bool currentFY = false, bool last12periods = false)
        {
            MLFSReportingPeriod period = await _periodData.GetPeriodById(currentMonthId);
            List<MLFSIncome> incomeLines = new List<MLFSIncome>();
            List<MLFSIncomeReport> reportLines = new List<MLFSIncomeReport>();
            if (!currentFY && !last12periods)
            {
                incomeLines = period.Receipts;
            }
            else
            {
                List<MLFSReportingPeriod> periods = new List<MLFSReportingPeriod>();
                if (currentFY)
                {
                    periods = await _periodData.GetFinancialYear(period);
                }
                else if (last12periods)
                {
                    periods = await _periodData.GetLast12Months(period);
                }
                incomeLines = periods.SelectMany(x => x.Receipts).ToList();
            }
            foreach (MLFSIncome income in incomeLines)
            {
                reportLines.Add(new MLFSIncomeReport(income));
            }
            return reportLines;
        }

        /// <summary>
        /// Returns a "pivoted" set of data by either advisor or organisation
        /// </summary>
        /// <param name="periodId">an array of integers representing the ids of the periods you want to report for</param>
        /// <param name="byAdvisor">If true then it will pivot on the advisor</param>
        /// <param name="byOrganisation">if true it will pivot on the organisation</param>
        /// <returns>The IncomeReport lines for the period pivoted</returns>
        [HttpGet]
        [Route("IncomeReport")]
        public async Task<List<IncomeReport>> IncomeReport(int[] periodId, bool byAdvisor = false, bool byOrganisation = false)
        {
            List<MLFSReportingPeriod> periods = new List<MLFSReportingPeriod>();
            List<IncomeReport> report = new List<IncomeReport>();

            for (int i = 0; i < periodId.Length; i++)
            {
                MLFSReportingPeriod period = await _periodData.GetPeriodById(periodId[i]);
                periods.Add(period);
            }
            
            if (byAdvisor && !byOrganisation)
            {
                report = ViewModels.IncomeReport.CreateReportByAdvisor(periods);
            }
            else if (byOrganisation && !byAdvisor)
            {
                report = ViewModels.IncomeReport.CreateReportByOrganisation(periods);
            }
            else
            {
                report = ViewModels.IncomeReport.CreateFromList(periods.SelectMany(x => x.Receipts).ToList());
            }
            return report;
        }

    }
}
