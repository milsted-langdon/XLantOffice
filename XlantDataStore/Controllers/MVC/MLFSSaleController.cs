using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XLantCore.Models;
using XLantDataStore;

namespace XLantDataStore.Controllers.MVC
{
    public class MLFSSaleController : Controller
    {
        private readonly Repository.IMLFSSaleRepository _salesData;
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;
        private readonly Repository.IMLFSClientRepository _clientData;

        public MLFSSaleController(XLantDbContext context)
        {
            _salesData = new Repository.MLFSSaleRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _clientData = new Repository.MLFSClientRepository();
        }

        // GET: MLFSSales
        public async Task<IActionResult> Index(int? periodId)
        {
            if (periodId == null)
            {
                return NotFound();
            }
            MLFSReportingPeriod period = await _periodData.GetPeriodById((int)periodId);
            if (period == null)
            {
                return NotFound();
            }
            List<MLFSSale> sales = await _salesData.GetSales(period);
            return View(sales);
        }

        public async Task<IActionResult> UpdateData(int? periodId)
        {
            if (periodId == null)
            {
                return NotFound();
            }
            MLFSReportingPeriod period = await _periodData.GetPeriodById((int)periodId);
            if (period == null)
            {
                return NotFound();
            }
            List<MLFSSale> sales = await _salesData.GetSales(period);

            for (int i = 0; i < sales.Count; i++)
            {
                MLFSSale sale = sales[i];
                if (!sale.InitialFeePass || !sale.OngoingFeePass)
                {
                    MLFSClient client = await _clientData.GetClient(sale.ClientId);
                    sale.AddClientData(client);
                    _salesData.Update(sale);
                }
            }

            return RedirectToAction("Index", "DirectorsReport", new { periodId = periodId });
        }
    }
}
