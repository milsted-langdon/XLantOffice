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
        private readonly XLantDbContext _context;

        public DataMapsController(XLantDbContext context)
        {
            _context = context;
        }

        // GET: DataMaps
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataMaps.ToListAsync());
        }

        // GET: DataMaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataMap = await _context.DataMaps
                .FirstOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Create([Bind("Id,ClassName,SourceName,InternalFieldName,ExternalFieldName")] DataMap dataMap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataMap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

            var dataMap = await _context.DataMaps.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassName,SourceName,InternalFieldName,ExternalFieldName")] DataMap dataMap)
        {
            if (id != dataMap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataMap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataMapExists(dataMap.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
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

            var dataMap = await _context.DataMaps
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var dataMap = await _context.DataMaps.FindAsync(id);
            _context.DataMaps.Remove(dataMap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataMapExists(int id)
        {
            return _context.DataMaps.Any(e => e.Id == id);
        }
    }
}
