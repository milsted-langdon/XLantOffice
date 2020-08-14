using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XLantCore.Models;
using XLantDataStore.ViewModels;
using XLantDataStore;

namespace XLantDataStore.Controllers.MVC
{
    public class DirectorsReportController : Controller
    {
        private readonly Repository.IMLFSSaleRepository _salesData;
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;
        
        public DirectorsReportController(XLantDbContext context)
        {
            _salesData = new Repository.MLFSSaleRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
        }

        // GET: Index
        public async Task<IActionResult> Index(int? periodId)
        {
            if (periodId == null)
            {
                return NotFound();
            }
            MLFSReportingPeriod period = await _periodData.GetPeriodById((int)periodId);
            List<MLFSSale> sales = await _salesData.GetSales(period);

            List<DirectorsReport> report = ViewModels.DirectorsReport.Create(sales.OrderBy(x => x.ClientName).ToList());

            return View(report);
        }
    }
}
