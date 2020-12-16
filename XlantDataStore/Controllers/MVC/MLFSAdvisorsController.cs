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
    public class MLFSAdvisorsController : Controller
    {
        private readonly Repository.IMLFSAdvisorRepository _advisorData;

        public MLFSAdvisorsController(XLantDbContext context)
        {
            
            _advisorData = new Repository.MLFSAdvisorRepository(context);
        }

        // GET: MLFSAdvisors
        public async Task<IActionResult> Index()
        {
            return View(await _advisorData.GetAdvisors());
        }

        // GET: MLFSAdvisors/Create
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Grade,Username,Department,Office,PrimaryID,Title,FirstName,LastName")] MLFSAdvisor mLFSAdvisor)
        {
            if (ModelState.IsValid)
            {
                _advisorData.Add(mLFSAdvisor);
                return RedirectToAction(nameof(Index));
            }
            return View(mLFSAdvisor);
        }

        // GET: MLFSAdvisors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mLFSAdvisor = await _advisorData.GetAdvisor((int)id);
            if (mLFSAdvisor == null)
            {
                return NotFound();
            }
            ViewBag.ReplacementAdvisorId = await _advisorData.SelectList();
            return View(mLFSAdvisor);
        }

        // POST: MLFSAdvisors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ReplacementAdvisorId,Grade,Username,Department,Office,Active,Id,PrimaryID,Title,FirstName,LastName")] MLFSAdvisor mLFSAdvisor)
        {
            if (id != mLFSAdvisor.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _advisorData.Update(mLFSAdvisor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mLFSAdvisor);
        }
    }
}
