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
        private readonly IMLFSReportingPeriodRepository _periodData;

        public MLFSReportingPeriodController(ILogger<IntelligentOfficeController> logger, XLantDbContext context)
        {
            _logger = logger;
            _periodData = new MLFSReportingPeriodRepository(context);
        }

        /// <summary>
        /// Add a period to the database
        /// </summary>
        /// <param name="period">The data for the period</param>
        /// <returns>0 if there is a problem otherwise the id of the new period</returns>
        [HttpPost]
        [Route("Put")]
        public async Task<int> Put([FromBody] MLFSReportingPeriod period)
        {
            if (period != null)
            {
                int periodId = await _periodData.InsertPeriod(period);
                return periodId;
            }
            return 0;
        }

        /// <summary>
        /// Get the last 12 periods based on the current date
        /// </summary>
        /// <returns>A list of periods</returns>
        [HttpGet]
        [Route("GetCurrentPeriods")]
        public async Task<List<MLFSReportingPeriod>> GetCurrentPeriods()
        {
            MLFSReportingPeriod period = await _periodData.GetCurrent();
            List<MLFSReportingPeriod> currentPeriods = await _periodData.GetLast12Months(period);
            return currentPeriods;
        }

        /// <summary>
        /// Returns a period based on the id provided
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                MLFSReportingPeriod period = await _periodData.GetPeriodById(periodId);
                return period;
            }
            return null;
        }

    }
}
