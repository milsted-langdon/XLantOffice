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
    public class MLFSReportingPeriodController : ControllerBase
    {
        private readonly ILogger<IntelligentOfficeController> _logger;
        private readonly IMLFSReportingPeriodRepository _periodRepository;

        public MLFSReportingPeriodController(ILogger<IntelligentOfficeController> logger, XLantDbContext context)
        {
            _logger = logger;
            _periodRepository = new MLFSReportingPeriodRepository(context);
        }

        [HttpPost]
        [Route("Put")]
        public void Put(MLFSReportingPeriod period)
        {
            if (period != null)
            {
                _periodRepository.InsertPeriod(period);
            }
        }

        [HttpGet]
        [Route("GetCurrentPeriods")]
        public async Task<List<MLFSReportingPeriod>> GetCurrentPeriods()
        {
            List<MLFSReportingPeriod> periods = await _periodRepository.GetPeriods();
            List<MLFSReportingPeriod> currentPeriods = new List<MLFSReportingPeriod>();
            DateTime date = DateTime.Now;
            string finYear = "";
            string lastYear = "";
            int startYear = 0;
            if (date.Month > 4)
            {
                startYear = date.Year;
            }
            else
            {
                startYear = date.AddYears(-1).Year;
            }
            finYear = startYear.ToString().Substring(2);
            lastYear = (startYear - 1).ToString().Substring(2) + "/" + finYear;
            finYear += "/" + (startYear + 1).ToString().Substring(2);
            currentPeriods = periods.Where(x => x.FinancialYear == finYear).ToList();
            if (periods.Where(x => x.FinancialYear == lastYear && x.Month == 4).Count() != 0)
            {
                currentPeriods.Add(periods.Where(x => x.FinancialYear == lastYear && x.Month == 4).FirstOrDefault());
            }
            return currentPeriods;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<MLFSReportingPeriod> Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return null;
            }
            if (int.TryParse(id, out int periodId))
            {
                MLFSReportingPeriod period = await _periodRepository.GetPeriodById(periodId);
                return period;
            }
            return null;
        }

    }
}
