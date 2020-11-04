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
    public class MLFSDebtorAdjustmentsController : Controller
    {
        private readonly Repository.IMLFSDebtorAdjustmentRepository _adjustmentData;
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;
        private readonly Repository.IMLFSSaleRepository _salesData;
        

        public MLFSDebtorAdjustmentsController(XLantDbContext context)
        {
            _adjustmentData = new Repository.MLFSDebtorAdjustmentRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _salesData = new Repository.MLFSSaleRepository(context);

        }

        // GET: MLFSDebtorAdjustments
        public async Task<IActionResult> Transactions(int debtorId)
        {
            List<MLFSDebtorAdjustment> adjs = await _adjustmentData.GetAdjustments(debtorId);
            return PartialView("Index", adjs);
        }

        public async Task<IActionResult> Index(int? periodId, int? advisorId, string type = "")
        {
            MLFSReportingPeriod period;
            if (periodId == null)
            {
                period = await _periodData.GetCurrent();
            }
            else
            {
                period = await _periodData.GetPeriodById((int)periodId);
            }
            if (period == null)
            {
                return NotFound();
            }
            List<MLFSDebtorAdjustment> adjs = await _adjustmentData.GetAdjustments(period);
            if (advisorId != null)
            {
                adjs = adjs.Where(x => x.Debtor.AdvisorId == advisorId).ToList();
            }
            if (type.ToLower() == "ntu")
            {
                adjs = adjs.Where(x => x.NotTakenUp).ToList();
            }
            else if (type.ToLower() == "variance")
            {
                adjs = adjs.Where(x => x.IsVariance).ToList();
            }
            else if (type.ToLower() == "variance")
            {
                adjs = adjs.Where(x => !x.IsVariance && !x.NotTakenUp && x.ReceiptId == null).ToList();
            }
            return View(adjs);
        }

        // GET: MLFSDebtorAdjustments/Create
        public async Task<IActionResult> CreateVariance(int debtorId)
        {
            MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment();
            MLFSSale debtor = await _salesData.GetSaleById(debtorId);
            adj.DebtorId = debtorId;
            adj.Amount = debtor.Outstanding * -1;
            adj.IsVariance = true;
            adj.NotTakenUp = false;
            ViewBag.ReportingPeriodId = await _periodData.SelectList();
            return PartialView("Create", adj);
        }

        // GET: MLFSDebtorAdjustments/Create
        public async Task<IActionResult> CreateAdjustment(int debtorId)
        {
            MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment();
            MLFSSale debtor = await _salesData.GetSaleById(debtorId);
            adj.DebtorId = debtorId;
            adj.Amount = debtor.Outstanding * -1;
            adj.IsVariance = false;
            adj.NotTakenUp = false;
            ViewBag.ReportingPeriodId = await _periodData.SelectList();
            return PartialView("Create", adj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ReportingPeriodId,DebtorId,ReceiptId,Amount,IsVariance,NotTakenUp")] MLFSDebtorAdjustment adj)
        {
            if (ModelState.IsValid)
            {
                _adjustmentData.Insert(adj);
            }
            else
            {
                //return error
            }
            return RedirectToAction("Index", "Debtor");
        }

        public async Task<IActionResult> NotTakenUp(int? debtorId)
        {
            if (debtorId == null)
            {
                return NotFound();
            }

            MLFSSale mLFSSale = await _salesData.GetSaleById((int)debtorId);
            if (mLFSSale == null)
            {
                return NotFound();
            }
            MLFSReportingPeriod period = await _periodData.GetCurrent();
            MLFSDebtorAdjustment adj = mLFSSale.CreateNTU(period);
            _adjustmentData.Insert(adj);
            return RedirectToAction("Index", "Debtor");
        }

        public async Task<IActionResult> Reverse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adj = await _adjustmentData.GetAdjustmentById((int)id);
            if (adj == null)
            {
                return NotFound();
            }
            MLFSDebtorAdjustment clone = adj.Clone();
            clone.Amount = adj.Amount * -1;
            adj.ReceiptId = null;
            _adjustmentData.Insert(clone);
            _adjustmentData.Update(adj);
            return RedirectToAction("Index", "Debtor");
        }

    }
}
