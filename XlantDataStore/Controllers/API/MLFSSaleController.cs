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

namespace XLantDataStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MLFSSaleController : ControllerBase
    {
        private readonly ILogger<IntelligentOfficeController> _logger;
        private readonly IMLFSReportingPeriodRepository _periodRepository;
        private readonly IMLFSSaleRepository _salesRepository;
        private readonly IMLFSIncomeRepository _incomeRepository;

        public MLFSSaleController(ILogger<IntelligentOfficeController> logger, XLantDbContext context)
        {
            _logger = logger;
            _periodRepository = new MLFSReportingPeriodRepository(context);
            _salesRepository = new MLFSSaleRepository(context);
            _incomeRepository = new MLFSIncomeRepository(context);
        }

        [HttpPost]
        [Route("PostMonthlyData")]
        public async Task<ActionResult> PostMonthlyData(IFormFile salesCSV, IFormFile planCSV, IFormFile fciCSV, string periodId)
        {
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
            if (periodId == null || !int.TryParse(periodId, out int pId))
            {
                return BadRequest();
            }
            DataTable feeDt = Tools.ConvertCSVToDataTable(newSalesFilePath);
            DataTable planDt = Tools.ConvertCSVToDataTable(newPlanFilePath);
            DataTable fciDt = Tools.ConvertCSVToDataTable(newFCIFilePath);
            //Get the period we are using
            MLFSReportingPeriod period = await _periodRepository.GetPeriodById(pId);
            //load data to database and get response
            await _salesRepository.UploadSalesForPeriod(period, feeDt, planDt);
            await _incomeRepository.UploadIncomeForPeriod(period, fciDt);
            return Ok();
        }

    }
}
