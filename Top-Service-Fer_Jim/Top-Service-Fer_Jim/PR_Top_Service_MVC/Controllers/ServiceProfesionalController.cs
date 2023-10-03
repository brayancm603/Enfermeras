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
    public class ServiceProfesionalController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public ServiceProfesionalController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: ServiceProfesional
        public async Task<IActionResult> Index(int? id)
        {
            int x = UserConfig.userLogin.IdUser;
            var topServiceBDOContext = _context.Services.Include(s => s.IdAdminNavigation).Include(s => s.IdProfessionalNavigation).Where(s => s.IdProfessional == x).Where(s=>s.IdService==id); 
            return View(await topServiceBDOContext.ToListAsync());
        }

        // GET: ServiceProfesional/Details/5
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

        // GET: ServiceProfesional/Create
        public IActionResult Create()
        {
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin");
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional");
            return View();
        }

        // POST: ServiceProfesional/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdService,IdAdmin,IdProfessional,Name,Description,Date,Status,Status_Service,DateTime_On_Service,DateTime_Off_Service,Latitude,Longitude")] Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin", service.IdAdmin);
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", service.IdProfessional);
            return View(service);
        }

        // GET: ServiceProfesional/Edit/5
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

        // POST: ServiceProfesional/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdService,IdAdmin,IdProfessional,Name,Description,Date,Status,IdCostumer,Status_Service,DateTime_On_Service,DateTime_Off_Service,Latitude,Longitude")] Service service)
        {

            if (id != service.IdService)
            {
                return NotFound();
            }
            if (service.Status_Service == null)
            {
                service.Status = 2;
                service.Status_Service = 1;
                service.DateTime_On_Service = DateTime.Now;
                service.Latitude = service.Latitude;
                service.Longitude = service.Longitude;
            }
            else
            {
                service.Status = 0;
                service.Status_Service = 2;
                service.DateTime_Off_Service = DateTime.Now;
            }
            ViewBag.service = service.Status;

           
                
            
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
                return RedirectToAction("Index", "Calendar");
            }
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin", service.IdAdmin);
            ViewData["IdProfessional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", service.IdProfessional);
            ViewData["IdCostumer"] = new SelectList(_context.Costumers, "IdCostumer", "IdCostumer", service.IdCostumer);
            return View(service);



        }

        private IActionResult Redirect(object value)
        {
            throw new NotImplementedException();
        }

        // GET: ServiceProfesional/Delete/5
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

        // POST: ServiceProfesional/Delete/5
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
                _context.Services.Remove(service);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return (_context.Services?.Any(e => e.IdService == id)).GetValueOrDefault();
        }


    }
}
