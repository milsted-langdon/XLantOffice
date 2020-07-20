using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XLantCore;
using XLantCore.Models;
using XLantDataStore;

namespace XLantDataStore.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class MLFSReportingPeriodsController : Controller
    {
        private readonly Repository.IMLFSReportingPeriodRepository _context;

        public MLFSReportingPeriodsController(XLantDbContext context)
        {
            _context = new Repository.MLFSReportingPeriodRepository(context);
        }

        [Route("Index")]
        // GET: MLFSReportingPeriods
        public async Task<IActionResult> Index()
        {
            return View(await _context.GetPeriods());
        }

        [Route("Details")]
        // GET: MLFSReportingPeriods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mLFSReportingPeriod = await _context.GetPeriodById((int)id);
            if (mLFSReportingPeriod == null)
            {
                return NotFound();
            }

            return View(mLFSReportingPeriod);
        }

        // GET: MLFSReportingPeriods/Create
        [Route("Create")]
        public IActionResult Create()
        {
            Dictionary<int, string> months = Tools.MonthsList();
            ViewBag.Months = new SelectList(months.AsEnumerable(), "Key", "Value");
            return View();
        }

        // POST: MLFSReportingPeriods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Month,Year")] MLFSReportingPeriod mLFSReportingPeriod)
        {
            if (ModelState.IsValid)
            {
                MLFSReportingPeriod period = new MLFSReportingPeriod(mLFSReportingPeriod.Month, mLFSReportingPeriod.Year);
                await _context.InsertPeriod(period);
                return RedirectToAction(nameof(Index));
            }
            return View(mLFSReportingPeriod);
        }

        // GET: MLFSReportingPeriods/Edit/5
        [Route("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mLFSReportingPeriod = await _context.GetPeriodById((int)id);
            if (mLFSReportingPeriod == null)
            {
                return NotFound();
            }
            return View(mLFSReportingPeriod);
        }

        // POST: MLFSReportingPeriods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Description,Month,Year,FinancialYear,ReportOrder")] MLFSReportingPeriod mLFSReportingPeriod)
        {
            if (id != mLFSReportingPeriod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mLFSReportingPeriod);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mLFSReportingPeriod);
        }

        // GET: MLFSReportingPeriods/Delete/5
        [Route("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mLFSReportingPeriod = await _context.GetPeriodById((int)id);
            if (mLFSReportingPeriod == null)
            {
                return NotFound();
            }
            return View(mLFSReportingPeriod);
        }

        // POST: MLFSReportingPeriods/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _context.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
