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
    public class DebtorController : Controller
    {
        private readonly Repository.IMLFSSaleRepository _debtorData;
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;
        private readonly Repository.IMLFSDebtorAdjustmentRepository _adjustmentData;

        public DebtorController(XLantDbContext context)
        {
            _debtorData = new Repository.MLFSSaleRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _adjustmentData = new Repository.MLFSDebtorAdjustmentRepository(context);
        }

        // GET: Debtor
        public async Task<IActionResult> Index()
        {
            return View(await _debtorData.GetDebtors());
        }

        // GET: Debtor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mLFSSale = await _debtorData.GetSaleById((int)id);
            if (mLFSSale == null)
            {
                return NotFound();
            }
            return View(mLFSSale);
        }

        public async Task<IActionResult> NotTakenUp(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MLFSSale mLFSSale = await _debtorData.GetSaleById((int)id);
            if (mLFSSale == null)
            {
                return NotFound();
            }
            MLFSReportingPeriod period = await _periodData.GetCurrent();
            MLFSDebtorAdjustment adj = mLFSSale.CreateNTU(period);
            _adjustmentData.Update(adj);
            return RedirectToAction("Index");
        }
    }
}
