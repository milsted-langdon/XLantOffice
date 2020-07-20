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
        private readonly IMLFSReportingPeriodRepository _periodRepository;
        private readonly IMLFSIncomeRepository _incomeRepository;

        public MLFSIncomeController(ILogger<IntelligentOfficeController> logger, XLantDbContext context)
        {
            _logger = logger;
            _periodRepository = new MLFSReportingPeriodRepository(context);
            _incomeRepository = new MLFSIncomeRepository(context);
        }

        [HttpPost]
        [Route("PostMonthlyIncome")]
        public async Task<List<MLFSIncome>> PostMonthlyIncome(IFormFile fciCSV, string periodId)
        {
            //convert our csv into datatable
            string newFCIFilePath = Path.GetTempFileName();
            if (fciCSV.Length > 0)
            {
                using (var fileStream = new FileStream(newFCIFilePath, FileMode.Create))
                {
                    await fciCSV.CopyToAsync(fileStream);
                }
            }
            else
            {
                //send response no file
            }
            int pId = 0;
            if (periodId == null || !int.TryParse(periodId, out pId))
            {
                //send response invalid id
            }
            DataTable fciDt = Tools.ConvertCSVToDataTable(newFCIFilePath);
            //Get the period we are using
            MLFSReportingPeriod period = await _periodRepository.GetPeriodById(pId);
            //load data to database and get response
            List<MLFSIncome> incomeList = await _incomeRepository.UploadIncomeForPeriod(period, fciDt);
            //return list of sales
            return incomeList;
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
            MLFSReportingPeriod period = await _periodRepository.GetPeriodById(currentMonthId);
            List<MLFSIncome> incomeLines = new List<MLFSIncome>();
            List<MLFSIncomeReport> reportLines = new List<MLFSIncomeReport>();
            if (!currentFY && !last12periods)
            {
                incomeLines = await _incomeRepository.GetIncome(period);
            }
            else
            {
                incomeLines = await _incomeRepository.GetIncome();
                List<MLFSReportingPeriod> periods = await _periodRepository.GetPeriods();
                if (currentFY)
                {
                    periods = periods.Where(x => x.FinancialYear == period.FinancialYear).ToList();
                }
                else if (last12periods)
                {
                    periods = periods.Where(x => x.FinancialYear == period.FinancialYear || (x.FinancialYear == period.PriorYear && x.ReportOrder > period.ReportOrder)).ToList();
                }
                else
                {
                    return null;
                }
                incomeLines = incomeLines.Where(x => x.MLFSReportPeriodId != null && periods.Select(y => y.Id).ToList().Contains((int)x.MLFSReportPeriodId)).ToList();
            }
            foreach (MLFSIncome income in incomeLines)
            {
                reportLines.Add(new MLFSIncomeReport(income));
            }
            return reportLines;
        }

    }
}
