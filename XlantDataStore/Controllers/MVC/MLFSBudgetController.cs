using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XLantCore.Models;
using XLantDataStore;
using XLantDataStore.ViewModels;

namespace XLantDataStore.Controllers.MVC
{
    public class MLFSBudgetController : Controller
    {
        private readonly Repository.IMLFSReportingPeriodRepository _periodData;
        private readonly Repository.IMLFSAdvisorRepository _advisorData;
        private readonly Repository.IMLFSBudgetRepository _budgetData;

        public MLFSBudgetController(XLantDbContext context)
        {
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _advisorData = new Repository.MLFSAdvisorRepository(context);
            _budgetData = new Repository.MLFSBudgetRepository(context);
        }

        // GET: Debtor
        public async Task<IActionResult> Index(int? advisorId, string financialYear)
        {
            List<MLFSReportingPeriod> periods = await _periodData.GetPeriods();
            SelectList yearList = new SelectList(periods.Select(x => x.FinancialYear).Distinct());
            ViewBag.Year = yearList;
            ViewBag.AdvisorId = await _advisorData.SelectList();
            if (advisorId == null)
            {
                BudgetReview review = new BudgetReview();
                return View(review);
            }
            else
            {
                if (String.IsNullOrEmpty(financialYear))
                {
                    financialYear = yearList.FirstOrDefault().Value;
                }
                MLFSAdvisor advisor = await _advisorData.GetAdvisor((int)advisorId);
                List<MLFSBudget> budgets = await _budgetData.GetBudgets(periods.Where(x => x.FinancialYear == financialYear).ToList());
                BudgetReview review = new BudgetReview(advisor, budgets, periods, financialYear);
                return View(review);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? advisorId, int? reportingPeriodId, string value)
        {
            if (reportingPeriodId == null)
            {
                return NotFound();
            }
            if (advisorId == null)
            {
                return NotFound();
            }
            if (decimal.TryParse(value, out decimal budget))
            {
                MLFSReportingPeriod period = await _periodData.GetPeriodById((int)reportingPeriodId);
                if (period == null)
                {
                    return NotFound();
                }
                List<MLFSBudget> budgets = await _budgetData.GetBudgets(period);
                MLFSBudget b = budgets.Where(x => x.AdvisorId == advisorId).FirstOrDefault();
                if (b == null)
                {
                    b = new MLFSBudget()
                    {
                        AdvisorId = (int)advisorId,
                        Budget = budget,
                        ReportingPeriodId = period.Id
                    };
                    _budgetData.Insert(b);
                }
                else
                {
                    b.Budget = budget;
                    _budgetData.Update(b);
                }
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
