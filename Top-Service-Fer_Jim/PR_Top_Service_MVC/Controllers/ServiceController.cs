  using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class ServiceController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public ServiceController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: Service
        public async Task<IActionResult> Index()
        {
            IQueryable<Service> service = from Service in _context.Services.Include(a=>a.IdProfessionalNavigation).Include(a=>a.IdProfessionalNavigation.IdProfesionalNavigation)

                                          where Service.Status == 1 && Service.IdAdmin == UserConfig.userLogin.IdUser
                                          select Service;

            return View(await service.ToListAsync());
        }

        // GET: Service/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.IdAdminNavigation)
                .Include(s => s.IdProfessionalNavigation)
                .FirstOrDefaultAsync(m => m.IdService == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Service/Create
        public IActionResult Create()
        {
          
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin");
            
            return View();
        }

        // POST: Service/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id,[Bind("IdService,IdAdmin,IdProfessional,idArea,Description,Date,Status")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.IdProfessional = id;
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin", service.IdAdmin);
           
            return View(service);
        }

        // GET: Service/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin", service.IdAdmin);
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", service.IdProfessional);
            ViewData["IdCostumer"] = new SelectList(_context.Costumers, "IdCostumer", "IdCostumer", service.IdCostumer);
            return View(service);
        }

        // POST: Service/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdService,IdAdmin,IdProfessional,IdCostumer,Description,Date,Status")] Service service)
        {
            if (id != service.IdService)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.IdService))
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
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin", service.IdAdmin);
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", service.IdProfessional);
            ViewData["IdCostumer"] = new SelectList(_context.Costumers, "IdCostumer", "IdCostumer", service.IdCostumer);
            return View(service);
        }

        // GET: Service/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Services == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.IdAdminNavigation)
                .Include(s => s.IdProfessionalNavigation)
                .FirstOrDefaultAsync(m => m.IdService == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Services == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.Services'  is null.");
            }
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                service.Status = 0;
                _context.Services.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.Services?.Any(e => e.IdService == id)).GetValueOrDefault();
        }



        #region view status service by client
        public async Task<IActionResult> ServiceByClient()
        {
            IQueryable<Service> service = from Service in _context.Services
                                          where Service.IdCostumer == UserConfig.userLogin.IdUser
                                          select Service;
           

            return View(await service.ToListAsync());
        }
        #endregion


    }
}
