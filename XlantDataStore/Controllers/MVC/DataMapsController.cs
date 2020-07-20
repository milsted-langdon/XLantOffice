using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using XLantCore.Models;
using XLantDataStore;

namespace XLantDataStore.Controllers
{
    public class DataMapsController : Controller
    {
        private readonly Repository.IDataMapRepository _dataMap;

        public DataMapsController(XLantDbContext context)
        {
            _dataMap = new Repository.DataMapRepository(context);
        }

        /// <summary>
        /// GET: Returns datamaps based on the filename given
        /// </summary>
        /// <param name="csvFile">the filename of the CSV you are mapping to an object</param>
        /// <returns>a List of DataMaps</returns>
        public async Task<IActionResult> Index(string csvFile)
        {
            return View(await _dataMap.GetMaps(csvFile));
        }

        // GET: DataMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dataMap = await _dataMap.GetDataMapById((int)id);
            if (dataMap == null)
            {
                return NotFound();
            }

            return View(dataMap);
        }

        // GET: DataMaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataMaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ClassName,SourceName,InternalFieldName,ExternalFieldName")] DataMap dataMap)
        {
            if (ModelState.IsValid)
            {
                _dataMap.Insert(dataMap);
                return RedirectToAction("Index", new { csvFile = dataMap.SourceName });
            }
            return View(dataMap);
        }

        // GET: DataMaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataMap = await _dataMap.GetDataMapById((int)id);
            if (dataMap == null)
            {
                return NotFound();
            }
            return View(dataMap);
        }

        // POST: DataMaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,ClassName,SourceName,InternalFieldName,ExternalFieldName")] DataMap dataMap)
        {
            if (id != dataMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataMap.Update(dataMap);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index", new { dataMap.SourceName });
            }
            return View(dataMap);
        }

        // GET: DataMaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataMap = await _dataMap.GetDataMapById((int)id);
            if (dataMap == null)
            {
                return NotFound();
            }

            return View(dataMap);
        }

        // POST: DataMaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataMap = await _dataMap.GetDataMapById((int)id);
            string source = dataMap.SourceName;
            _dataMap.Delete(id);
            return RedirectToAction("Index", new { csvFile = source });
        }
    }
}
