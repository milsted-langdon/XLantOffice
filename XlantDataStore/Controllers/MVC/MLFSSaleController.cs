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
        private readonly Repository.IMLFSAdvisorRepository _advisorData;
        private readonly Repository.IMLFSDebtorAdjustmentRepository _adjData;
        private readonly Repository.IMLFSIncomeRepository _incomeData;

        public MLFSSaleController(XLantDbContext context)
        {
            _salesData = new Repository.MLFSSaleRepository(context);
            _periodData = new Repository.MLFSReportingPeriodRepository(context);
            _clientData = new Repository.MLFSClientRepository();
            _advisorData = new Repository.MLFSAdvisorRepository(context);
            _adjData = new Repository.MLFSDebtorAdjustmentRepository(context);
            _incomeData = new Repository.MLFSIncomeRepository(context);
        }

        // GET: MLFSSales
        public async Task<IActionResult> Index(int? periodId, int? advisorId = null)
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
            if (advisorId != null)
            {
                sales = sales.Where(x => x.AdvisorId == advisorId).ToList();
            }
            return View(sales);
        }

        // GET: MLFSSales/VAT
        public async Task<IActionResult> VAT(int? periodId)
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
            return View("Index", sales.Where(x => x.VAT != 0));
        }

        [HttpPost]
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
                MLFSClient client = await _clientData.GetClient(sale.ClientId);
                sale.AddClientData(client);
                _salesData.Update(sale);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.ReportingPeriodId = await _periodData.SelectList();
            ViewBag.AdvisorId = await _advisorData.SelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(MLFSSale sale)
        {
            sale.Advisor = await _advisorData.GetAdvisor(sale.AdvisorId);
            sale.Organisation = sale.Advisor.Department;
            sale.IOId = sale.IOReference.Substring(2);
            int newId = await _salesData.Add(sale);
            if (sale.ReportingPeriodId == null)
            {
                return NotFound();
            }
            sale.ReportingPeriod = await _periodData.GetPeriodById((int)sale.ReportingPeriodId);
            List<MLFSReportingPeriod> periods = await _periodData.GetPeriods();
            MLFSReportingPeriod period;
            if (sale.ReportingPeriod.ReportOrder == 12)
            {
                period = periods.Where(x => x.Year == sale.ReportingPeriod.Year && x.ReportOrder == 1).FirstOrDefault();
            }
            else
            {
                period = periods.Where(x => x.FinancialYear == sale.ReportingPeriod.FinancialYear && x.ReportOrder == sale.ReportingPeriod.ReportOrder + 1).FirstOrDefault();
            }
            //create an entry in the next month to balance out
            MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment()
            {
                DebtorId = newId,
                Amount = sale.GrossAmount *-1,
                ReportingPeriodId = period.Id
            };
            _adjData.Insert(adj);
            return RedirectToAction("Index", "MLFSReport");
        }

        [HttpPost]
        public async Task<IActionResult> CreateFromIncome(int incomeId)
        {
            MLFSIncome income = await _incomeData.GetIncomeById(incomeId);
            if (income.MLFSDebtorAdjustment != null)
            {
                return NotFound();
            }
            MLFSSale sale = new MLFSSale(income);
            MLFSDebtorAdjustment adj = new MLFSDebtorAdjustment(sale, income);
            await _salesData.Add(sale);
            _adjData.Insert(adj);
            return Ok();
        }

        [HttpPost]
        public async Task<JsonResult> ClearVAT(int? debtorId)
        {
            if (debtorId == null)
            {
                return Json("Failed");
            }
            MLFSSale debtor = await _salesData.GetSaleById((int)debtorId);
            if (debtor == null)
            {
                return Json("Failed");
            }
            debtor.VAT = 0;
            _salesData.Update(debtor);
            return Json("Success");
        }
    }
}
