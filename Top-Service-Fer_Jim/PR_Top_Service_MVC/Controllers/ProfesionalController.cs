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
    public class ProfesionalController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public ProfesionalController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: Profesional
        public async Task<IActionResult> Index()
        {
            IQueryable<Profesional> profesional = from Profesional in _context.Profesionals

                                         
                                          select Profesional;

            return View(await profesional.ToListAsync());
        }

        // GET: Profesional/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Profesionals == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionals
                .Include(p => p.IdProfesionalNavigation)
                .FirstOrDefaultAsync(m => m.IdProfesional == id);
            if (profesional == null)
            {
                return NotFound();
            }

            return View(profesional);
        }

        // GET: Profesional/Create
        public IActionResult Create()
        {
           
            ViewData["IdProfesional"] = new SelectList(_context.People, "IdPerson", "IdPerson");
            return View();
        }

        // POST: Profesional/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProfesional,idArea,Ocupation,Birthdate")] Profesional profesional)
        {
            if (ModelState.IsValid)
            {
                _context.Add(profesional);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          
            ViewData["IdProfesional"] = new SelectList(_context.People, "IdPerson", "IdPerson", profesional.IdProfesional);
            return View(profesional);
        }

        // GET: Profesional/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Profesionals == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionals.FindAsync(id);
            if (profesional == null)
            {
                return NotFound();
            }
            ViewData["IdProfesional"] = new SelectList(_context.People, "IdPerson", "IdPerson", profesional.IdProfesional);
            return View(profesional);
        }

        // POST: Profesional/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProfesional,Ocupation,Birthdate")] Profesional profesional)
        {
            if (id != profesional.IdProfesional)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profesional);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfesionalExists(profesional.IdProfesional))
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
            ViewData["IdProfesional"] = new SelectList(_context.People, "IdPerson", "IdPerson", profesional.IdProfesional);
            return View(profesional);
        }

        // GET: Profesional/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Profesionals == null)
            {
                return NotFound();
            }

            var profesional = await _context.Profesionals
                .Include(p => p.IdProfesionalNavigation)
                .FirstOrDefaultAsync(m => m.IdProfesional == id);
            if (profesional == null)
            {
                return NotFound();
            }

            return View(profesional);
        }

        // POST: Profesional/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Profesionals == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.Profesionals'  is null.");
            }
            var profesional = await _context.Profesionals.FindAsync(id);
            if (profesional != null)
            {
                _context.Profesionals.Remove(profesional);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfesionalExists(int id)
        {
          return (_context.Profesionals?.Any(e => e.IdProfesional == id)).GetValueOrDefault();
        }
    }
}
