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
    public class PeopleController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public PeopleController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            var topServiceBDOContext = _context.People.Include(p => p.IdDepartmentNavigation);
            return View(await topServiceBDOContext.ToListAsync());
        }

        public async Task<IActionResult> ProfessionalArea(int id)
        {
            IQueryable<Postulation> person = from Postulation in _context.Postulations.Include(a => a.IdProfessionalNavigation.IdProfesionalNavigation)
                                        join Profesional in _context.Profesionals on Postulation.IdProfessional equals Profesional.IdProfesional
                                        join AreaAttribute in _context.AreaProfesional on Profesional.IdProfesional equals AreaAttribute.IdProfesional
                                        join JobArea in _context.JobArea on AreaAttribute.idArea equals JobArea.idArea
                                        join Person in _context.People on Profesional.IdProfesional equals Person.IdPerson
                                        where Postulation.idArea == id && Postulation.Status == 2
                                        select Postulation;



            return View(await person.ToListAsync());
        }


        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.IdDepartmentNavigation)
                .FirstOrDefaultAsync(m => m.IdPerson == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: People/Create
        public IActionResult Create()
        {
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "IdDepartment");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPerson,Name,LastName,SecondLastName,IdDepartment,status,deleted")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "IdDepartment", person.IdDepartment);
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "IdDepartment", person.IdDepartment);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPerson,Name,LastName,SecondLastName,IdDepartment,status,deleted")] Person person)
        {
            if (id != person.IdPerson)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.IdPerson))
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
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "IdDepartment", person.IdDepartment);
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .Include(p => p.IdDepartmentNavigation)
                .FirstOrDefaultAsync(m => m.IdPerson == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.People'  is null.");
            }
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
          return (_context.People?.Any(e => e.IdPerson == id)).GetValueOrDefault();
        }
    }
}
