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
    
    public class PostulationController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public PostulationController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: Postulation
        public async Task<IActionResult> Index()
        {
            var topServiceBDOContext = _context.Postulations.Include(p => p.IdProfessionalNavigation);
            return View(await topServiceBDOContext.ToListAsync());
        }

        // GET: Postulation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Postulations == null)
            {
                return NotFound();
            }

            var postulation = await _context.Postulations
                .Include(p => p.IdProfessionalNavigation)
                .FirstOrDefaultAsync(m => m.IdPostulation == id);
            if (postulation == null)
            {
                return NotFound();
            }

            return View(postulation);
        }

        // GET: Postulation/Create
        public IActionResult Create()
        {
            ViewData["idArea"] = new SelectList(_context.JobArea, "idArea", "Name");
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional");
            return View();
        }

        // POST: Postulation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPostulation,ProfessionalTitles,Certification,WorkExperience,Date,Address,Status,IdProfessional,idArea")] Postulation postulation)
        {
            if (ModelState.IsValid)
            {
                postulation.Status = 1;
                postulation.IdProfessional = UserConfig.userLogin.IdUser;
                _context.Add(postulation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["idArea"] = new SelectList(_context.JobArea, "idArea", "Name",postulation.idArea);
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", postulation.IdProfessional);
            return RedirectToAction("Index", "Home");
        }



        // GET: Postulation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Postulations == null)
            {
                return NotFound();
            }

            var postulation = await _context.Postulations.FindAsync(id);
            if (postulation == null)
            {
                return NotFound();
            }
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", postulation.IdProfessional);
            return View(postulation);
        }

        // POST: Postulation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPostulation,ProfessionalTitles,Certification,WorkExperience,Date,Address,Status,IdProfessional")] Postulation postulation)
        {
            if (id != postulation.IdPostulation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postulation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostulationExists(postulation.IdPostulation))
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
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", postulation.IdProfessional);
            return View(postulation);
        }

        // GET: Postulation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Postulations == null)
            {
                return NotFound();
            }

            var postulation = await _context.Postulations
                .Include(p => p.IdProfessionalNavigation)
                .FirstOrDefaultAsync(m => m.IdPostulation == id);
            if (postulation == null)
            {
                return NotFound();
            }

            return View(postulation);
        }

        // POST: Postulation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Postulations == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.Postulations'  is null.");
            }
            var postulation = await _context.Postulations.FindAsync(id);
            if (postulation != null)
            {
                _context.Postulations.Remove(postulation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostulationExists(int id)
        {
          return (_context.Postulations?.Any(e => e.IdPostulation == id)).GetValueOrDefault();
        }
    }
}
