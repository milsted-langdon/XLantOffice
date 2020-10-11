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

    }
}
