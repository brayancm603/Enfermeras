using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Controllers
{
    [Authorize]
    public class JobAreaController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public JobAreaController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: JobArea
        public async Task<IActionResult> Index()
        {
              return _context.JobArea != null ? 
                          View(await _context.JobArea.ToListAsync()) :
                          Problem("Entity set 'TopServiceBDOContext.JobArea'  is null.");
        }

        // GET: JobArea/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.JobArea == null)
            {
                return NotFound();
            }

            var jobArea = await _context.JobArea
                .FirstOrDefaultAsync(m => m.idArea == id);
            if (jobArea == null)
            {
                return NotFound();
            }

            return View(jobArea);
        }

        // GET: JobArea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobArea/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdArea,Name,Description")] JobArea jobArea)
        {
            if (ModelState.IsValid)
            {
                jobArea.deleted = 0;
                _context.Add(jobArea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobArea);
        }

        // GET: JobArea/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.JobArea == null)
            {
                return NotFound();
            }

            var jobArea = await _context.JobArea.FindAsync(id);
            if (jobArea == null)
            {
                return NotFound();
            }
            return View(jobArea);
        }

        // POST: JobArea/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(byte id, JobArea jobArea)
        {
            if (ModelState.IsValid)
            {
                JobArea s = _context.JobArea.First(x => x.idArea == id);
                s.Name = jobArea.Name;
                s.Description = jobArea.Description;
                _context.Update(s);
                _context.SaveChanges();
                return RedirectToAction("Index", "JobArea");

            }

            return RedirectToAction("Index", "JobArea");
        }

        // GET: JobArea/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.JobArea == null)
            {
                return NotFound();
            }

            var jobArea = await _context.JobArea
                .FirstOrDefaultAsync(m => m.idArea == id);
            if (jobArea == null)
            {
                return NotFound();
            }

            return View(jobArea);
        }

        // POST: JobArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
          
            var jobArea = await _context.JobArea.FindAsync(id);
            if (jobArea != null)
            {
                jobArea.deleted = 1;
                _context.Update(jobArea);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobAreaExists(byte id)
        {
          return (_context.JobArea?.Any(e => e.idArea == id)).GetValueOrDefault();
        }
    }
}
