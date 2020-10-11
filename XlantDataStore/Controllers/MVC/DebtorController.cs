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
        private readonly Repository.IMLFSIncomeRepository _incomeData;

        public DebtorController(XLantDbContext context)
        {
            _debtorData = new Repository.MLFSSaleRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _adjustmentData = new Repository.MLFSDebtorAdjustmentRepository(context);
            _incomeData = new Repository.MLFSIncomeRepository(context);
        }

        // GET: Debtor
        public async Task<IActionResult> Index(int? periodId)
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
            return View(await _debtorData.GetDebtors(period));
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

        public async Task<IActionResult> Match(int? id, int? debtorId)
        {
            if (debtorId == null || id == null)
            {
                return NotFound();
            }
            MLFSSale debtor = await _debtorData.GetSaleById((int)debtorId);
            MLFSIncome receipt = await _incomeData.GetIncomeById((int)id);
            if (debtor == null || receipt == null)
            {
                return NotFound();
            }
            List<MLFSDebtorAdjustment> adjs = new List<MLFSDebtorAdjustment>();
            adjs.Add(new MLFSDebtorAdjustment(debtor, receipt));
            if (debtor.Outstanding != 0  && (debtor.Outstanding < 0 || debtor.Outstanding/debtor.GrossAmount < (decimal)0.005))
            {
                adjs.Add(debtor.ClearToVariance(receipt.ReportingPeriod));
            }
            _adjustmentData.InsertList(adjs);
            return Ok();
        }

        public async Task<IActionResult> CheckForMatches()
        {
            MLFSReportingPeriod period = await _periodData.GetCurrent();
            List<MLFSSale> debtors = await _debtorData.GetDebtors(period);
            List<MLFSIncome> income = await _incomeData.GetIncome();
            income = income.Where(x => x.MLFSDebtorAdjustment == null && x.IncomeType.Contains("Initial")).ToList();
            List<MLFSDebtorAdjustment> adjs = MLFSSale.CheckForReceipts(debtors, income);
            _adjustmentData.InsertList(adjs);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return NotFound();
            }
            List<MLFSSale> sales = await _debtorData.Search(searchTerm);
            return View("Index", sales);
        }
    }
}
