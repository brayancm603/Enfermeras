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
    public class ServiceReceiptController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public ServiceReceiptController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: ServiceReceipt
        public async Task<IActionResult> Index()
        {

            IQueryable<Service> service = from Service in _context.Services.Include(a => a.IdCostumerNavigation).Include(a => a.ProfesionalS.IdPersonNavigation).Include(a => a.Receipt)
                                          join costumer in _context.Costumers on Service.IdCostumer equals costumer.IdCostumer
                                          join person in _context.People on costumer.IdCostumer equals person.IdPerson
                                          where person.IdDepartment == UserConfig.DepPer  && Service.Status == 0 && Service.Status_Service != 2 && Service.Status_Service != 1
                                          select Service;

            return View(await service.ToListAsync());
            //return _context.Services != null ?
            //            View(await _context.Services.Where(x => x.Status == 0 && x.Status_Service!= 2 && x.Status_Service !=1).Include(x=>x.ProfesionalS.IdPersonNavigation).Include(x=>x.Receipt).ToListAsync()) :
            //            Problem("Entity set 'TopServiceBDOContext.ServiceReceipt'  is null.");
        }


        public async Task<IActionResult> Receipt()
        {
            IQueryable<Service> service = from Service in _context.Services.Include(a => a.IdCostumerNavigation).Include(a => a.IdCostumerNavigation.IdCostumerNavigation).Include(a => a.Receipt)

                                          where  Service.IdCostumer == UserConfig.userLogin.IdUser
                                          select Service;

            return View(await service.ToListAsync());
        }
        // GET: ServiceReceipt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ServiceReceipt == null)
            {
                return NotFound();
            }

            var serviceReceipt = await _context.ServiceReceipt
                .FirstOrDefaultAsync(m => m.IdService == id);
            if (serviceReceipt == null)
            {
                return NotFound();
            }

            return View(serviceReceipt);
        }

        // GET: ServiceReceipt/Create
        public IActionResult Create()
        {
          
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin");
            ViewData["IdCostumer"] = new SelectList(_context.Costumers.OrderBy(x=>x.Ci), "IdCostumer", "Ci");
           

            return View();
        }

        // POST: ServiceReceipt/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("IdService,IdAdmin,IdProfessional,IdCostumer,deleted,Description,Date,Status,IdReceipt,DescriptionReceipt,Total")] ServiceReceipt serviceReceipt)
        {
            if (ModelState.IsValid)
            {
                Service s = new()
                {
                    IdProfessional = UserConfig.userLogin.IdUser,
                    IdCostumer = serviceReceipt.IdCostumer,

                    Description = serviceReceipt.Description,
                    Date = serviceReceipt.Date,
                    Status = 0,
                    deleted = 0
                };
                _context.Add(s);
                await _context.SaveChangesAsync();

                Receipt r = new()
                {
                    IdReceipt = s.IdService,
                    Description = serviceReceipt.DescriptionReceipt,
                    Total = serviceReceipt.Total


                };
                _context.Add(r);
                await _context.SaveChangesAsync();

            }
          
            ViewData["IdAdmin"] = new SelectList(_context.Admins, "IdAdmin", "IdAdmin", serviceReceipt.IdAdmin);
            ViewData["IdCostumer"] = new SelectList(_context.Costumers.OrderBy(x => x.Ci), "IdCostumer", "Ci", serviceReceipt.IdCostumer);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> EditService(int id)
        {


            if (ModelState.IsValid)
            {

                var datos = await _context.Services.FindAsync(id);
                datos.IdService = id;
                datos.IdCostumer = datos.IdCostumer;
                datos.IdProfessional = datos.IdProfessional;
                datos.Description = datos.Description;
                datos.Status = 1;
                datos.IdAdmin = UserConfig.userLogin.IdUser;
                datos.Date = datos.Date;
                datos.Status_Service = datos.Status_Service;
                datos.DateTime_On_Service = datos.DateTime_On_Service;
                datos.DateTime_Off_Service = datos.DateTime_Off_Service;
                datos.Latitude = datos.Latitude;
                datos.Longitude = datos.Longitude;
                datos.deleted = datos.deleted;
                _context.Update(datos);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "ServiceReceipt");
            }
            return RedirectToAction("Index", "ServiceReceipt");
        }

        // GET: ServiceReceipt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ServiceReceipt == null)
            {
                return NotFound();
            }

            var serviceReceipt = await _context.ServiceReceipt.FindAsync(id);
            if (serviceReceipt == null)
            {
                return NotFound();
            }
            return View(serviceReceipt);
        }

        // POST: ServiceReceipt/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdService,IdAdmin,IdProfessional,IdCostumer,Name,Description,Date,Status,IdReceipt,DescriptionReceipt,Total")] ServiceReceipt serviceReceipt)
        {
            if (id != serviceReceipt.IdService)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceReceipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceReceiptExists(serviceReceipt.IdService))
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
            return View(serviceReceipt);
        }

        // GET: ServiceReceipt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ServiceReceipt == null)
            {
                return NotFound();
            }

            var serviceReceipt = await _context.ServiceReceipt
                .FirstOrDefaultAsync(m => m.IdService == id);
            if (serviceReceipt == null)
            {
                return NotFound();
            }

            return View(serviceReceipt);
        }

        // POST: ServiceReceipt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ServiceReceipt == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.ServiceReceipt'  is null.");
            }
            var serviceReceipt = await _context.ServiceReceipt.FindAsync(id);
            if (serviceReceipt != null)
            {
                _context.ServiceReceipt.Remove(serviceReceipt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceReceiptExists(int id)
        {
            return (_context.ServiceReceipt?.Any(e => e.IdService == id)).GetValueOrDefault();
        }
    }
}
