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
            return _context.ServiceReceipt != null ?
                        View(await _context.ServiceReceipt.ToListAsync()) :
                        Problem("Entity set 'TopServiceBDOContext.ServiceReceipt'  is null.");
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
                    IdAdmin = UserConfig.userLogin.IdUser,
                    IdProfessional = id,
                    IdCostumer = serviceReceipt.IdCostumer,
                    
                    Description = serviceReceipt.Description,
                    Date = serviceReceipt.Date,
                    Status = 1,
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
            return RedirectToAction("Index", "Service");
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
