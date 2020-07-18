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

    }
}
