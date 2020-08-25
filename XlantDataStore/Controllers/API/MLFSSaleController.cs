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
    public class MLFSSaleController : ControllerBase
    {
        private readonly ILogger<IntelligentOfficeController> _logger;
        private readonly IMLFSReportingPeriodRepository _periodData;
        private readonly IMLFSSaleRepository _salesData;
        private readonly IMLFSIncomeRepository _incomeData;
        private readonly IMLFSDebtorAdjustmentRepository _adjData;

        public MLFSSaleController(ILogger<IntelligentOfficeController> logger, XLantDbContext context)
        {
            _logger = logger;
            _periodData = new MLFSReportingPeriodRepository(context);
            _salesData = new MLFSSaleRepository(context);
            _incomeData = new MLFSIncomeRepository(context);
            _adjData = new MLFSDebtorAdjustmentRepository(context);
        }

        /// <summary>
        /// This handles the upload from the CSVs to the database, would be nice to move some of this away from the controller
        /// </summary>
        /// <param name="salesCSV">The CSV containing invoices raised</param>
        /// <param name="planCSV">The CSV containing plans for the last few months</param>
        /// <param name="fciCSV">The CSV containing income received</param>
        /// <param name="periodId">The Id of the period by which the above is represented</param>
        /// <returns>Ok if all went well</returns>
        [HttpPost]
        [Route("PostMonthlyData")]
        public async Task<ActionResult> PostMonthlyData(IFormFile salesCSV, IFormFile planCSV, IFormFile fciCSV, string periodId)
        {
            //check we have a valid period
            if (periodId == null || !int.TryParse(periodId, out int pId))
            {
                return BadRequest();
            }
            //Get the period we are using
            MLFSReportingPeriod period = await _periodData.GetPeriodById(pId);
            if (period == null)
            {
                return BadRequest();
            }

            //convert our csvs into datatables
            string newSalesFilePath = Path.GetTempFileName();
            string newPlanFilePath = Path.GetTempFileName();
            string newFCIFilePath = Path.GetTempFileName();
            if (salesCSV.Length > 0)
            {
                using (var fileStream = new FileStream(newSalesFilePath, FileMode.Create))
                {
                    await salesCSV.CopyToAsync(fileStream);
                }
            }
            else
            {
                return NotFound();
            }
            if (planCSV.Length > 0)
            {
                using (var fileStream = new FileStream(newPlanFilePath, FileMode.Create))
                {
                    await planCSV.CopyToAsync(fileStream);
                }
            }
            else
            {
                return NotFound();
            }
            if (fciCSV.Length > 0)
            {
                using (var fileStream = new FileStream(newFCIFilePath, FileMode.Create))
                {
                    await fciCSV.CopyToAsync(fileStream);
                }
            }
            else
            {
                return NotFound();
            }
            DataTable feeDt = Tools.ConvertCSVToDataTable(newSalesFilePath);
            DataTable planDt = Tools.ConvertCSVToDataTable(newPlanFilePath);
            DataTable fciDt = Tools.ConvertCSVToDataTable(newFCIFilePath);
            
            //load data to database and get response
            await _salesData.UploadSalesForPeriod(period, feeDt, planDt);
            List<MLFSIncome> receipts = await _incomeData.UploadIncomeForPeriod(period, fciDt);
            await _incomeData.UpdateClientOnboardDate(period);

            //Allocate Receipts
            List<MLFSSale> debtors = await _salesData.GetDebtors();
            List<MLFSDebtorAdjustment> adjs = MLFSSale.CheckForReceipts(debtors, receipts.Where(x => x.IncomeType.Contains("Initial")).ToList());
            _adjData.InsertList(adjs);
            return Ok();
        }

    }
}
