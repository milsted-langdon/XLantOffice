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
    public class MLFSIncomeController : Controller
    {
        private readonly Repository.IMLFSIncomeRepository _incomeData;
        private readonly Repository.IMLFSSaleRepository _salesData;
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;

        public MLFSIncomeController(XLantDbContext context)
        {
            _incomeData = new Repository.MLFSIncomeRepository(context);
            _salesData = new Repository.MLFSSaleRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
        }

        // GET: MLFSSales
        public async Task<IActionResult> Index(int? periodId, string split="", int? advisorId = null)
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
            List<MLFSIncome> income = await _incomeData.GetIncome(period, advisorId);
            if (split.ToLower() == "unallocated")
            {
                income = income.Where(x => x.MLFSDebtorAdjustment == null && x.IsNewBusiness).ToList();
            }
            else if (split.ToLower() == "allocated")
            {
                income = income.Where(x => x.MLFSDebtorAdjustment != null && x.IsNewBusiness).ToList();
            }
            else if (split.ToLower() == "initial")
            {
                income = income.Where(x => x.IsNewBusiness).ToList();
            }
            else if (split.ToLower() == "recurring")
            {
                income = income.Where(x => !x.IsNewBusiness).ToList();
            }
            else if (split.ToLower() == "adjustment")
            {
                income = income.Where(x => x.IsAdjustment).ToList();
            }
            else if (split.ToLower() == "clawback")
            {
                income = income.Where(x => x.IsClawBack).ToList();
            }

            return View(income);
        }

        /// <summary>
        /// Displays a list of potential matches for a debtor
        /// </summary>
        /// <param name="debtorId">the Id of the debtor we are trying to match</param>
        /// <returns>Partial view</returns>
        public async Task<IActionResult> PotentialDebtorMatches(int? debtorId)
        {
            if (debtorId == null)
            {
                return NotFound();
            }
            MLFSSale debtor = await _salesData.GetSaleById((int)debtorId);
            if (debtor == null)
            {
                return NotFound();
            }
            List<MLFSIncome> matches = await _incomeData.PotentialDebtorMatches(debtor);
            ViewBag.Debtor = debtor;
            return PartialView("_SelectIndex", matches);
        }

        /// <summary>
        /// Displays a list of unmatched new business income.  If a debtor is supplied it will allow matching.  If a period is provided it will limit the list 
        /// </summary>
        /// <param name="debtorId">the Id of the debtor we are trying to match</param>
        /// <returns>Partial view</returns>
        public async Task<IActionResult> Unmatched(int? debtorId, int? periodId = null)
        {
            List<MLFSIncome> income = await _incomeData.GetUnMatchedIncome();
            if (debtorId != null)
            {
                MLFSSale debtor = await _salesData.GetSaleById((int)debtorId);
                ViewBag.Debtor = debtor;
            }
            if (periodId != null)
            {
                income = income.Where(x => x.ReportingPeriodId == periodId).ToList();
            }
            return PartialView("_SelectIndex", income);
        }
    }
}
