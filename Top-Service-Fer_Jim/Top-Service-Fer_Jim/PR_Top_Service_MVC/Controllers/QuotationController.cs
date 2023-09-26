using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;
using PR_Top_Service_MVC.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace PR_Top_Service_MVC.Controllers
{
    [Authorize]
    public class QuotationController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public QuotationController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: Quotation
        public async Task<IActionResult> Index()
        {
            var topServiceBDOContext = _context.Quotations.Include(q => q.IdCostumerNavigation);
            return View(await topServiceBDOContext.ToListAsync());
        }

        public async Task<IActionResult> ConfirmQuotation()
        {
            IQueryable<Quotation> person = from Quotation in _context.Quotations.Include(a => a.IdCostumerNavigation).Include(a => a.IdCostumerNavigation.IdCostumerNavigation)
                                        join Costumer in _context.Costumers on Quotation.IdCostumer equals Costumer.IdCostumer
                                        join Person in _context.People on Costumer.IdCostumer equals Person.IdPerson
                                        
                                        where Quotation.IdProfesional ==UserConfig.userLogin.IdUser && Quotation.Status == 1 
                                           select Quotation;



            return View(await person.ToListAsync());
        }


        public async Task<IActionResult> StateQuotation()
        {
            IQueryable<Quotation> person = from Quotation in _context.Quotations.Include(a => a.IdProfesionalNavigation).Include(a => a.IdProfesionalNavigation.IdProfesionalNavigation)
                                           join Costumer in _context.Costumers on Quotation.IdCostumer equals Costumer.IdCostumer
                                           join Person in _context.People on Costumer.IdCostumer equals Person.IdPerson

                                           where Quotation.IdCostumer == UserConfig.userLogin.IdUser && Quotation.Status >1

                                           
                                           select Quotation;



            return View(await person.ToListAsync());
        }
        // GET: Quotation/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .Include(q => q.IdCostumerNavigation)
                .FirstOrDefaultAsync(m => m.IdQuotation == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // GET: Quotation/Create
        public IActionResult Create()
        {

            ViewData["IdCostumer"] = new SelectList(_context.Costumers, "IdCostumer", "IdCostumer");
            ViewData["IdProfesional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional");
            return View();
        }

        // POST: Quotation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id ,[Bind("IdQuotation,IdCostumer,IdProfesional,Date,Description,Status")] Quotation quotation)
        {
            if (ModelState.IsValid)
            {
                quotation.IdProfesional = id;
                quotation.IdCostumer = UserConfig.userLogin.IdUser;
              


                _context.Add(quotation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index" , "JobArea");
            }
            ViewData["IdCostumer"] = new SelectList(_context.Costumers, "IdCostumer", "IdCostumer", quotation.IdCostumer);
            ViewData["IdProfesional"] = new SelectList(_context.Profesionals, "IdProfesional", "IdProfesional", quotation.IdProfesional);

            return RedirectToAction("Index", "JobArea");
        }

        // GET: Quotation/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation == null)
            {
                return NotFound();
            }
            ViewData["IdCostumer"] = new SelectList(_context.Costumers, "IdCostumer", "IdCostumer", quotation.IdCostumer);
            return View(quotation);
        }

        // POST: Quotation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdQuotation,IdCostumer,IdProfesional,Date,Service,Description,Status")] Quotation quotation)
        {
            if (id != quotation.IdQuotation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quotation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuotationExists(quotation.IdQuotation))
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
            ViewData["IdCostumer"] = new SelectList(_context.Costumers, "IdCostumer", "IdCostumer", quotation.IdCostumer);
            return View(quotation);
        }

        
        public async Task<IActionResult> Accept(int id )
        {
            var quotation = await _context.Quotations.FirstAsync(x => x.IdQuotation == id);
               
                    quotation.Status = 2;
                    _context.Update(quotation);
                    await _context.SaveChangesAsync();
               
                  
                
                

            return RedirectToAction("ConfirmQuotation", "Quotation");
        }


        public async Task<IActionResult> Reject(int id)
        {
            var quotation = await _context.Quotations.FirstAsync(x => x.IdQuotation == id);

            quotation.Status = 3;
            _context.Update(quotation);
            await _context.SaveChangesAsync();





            return RedirectToAction("ConfirmQuotation", "Quotation");
        }


       
        // GET: Quotation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quotations == null)
            {
                return NotFound();
            }

            var quotation = await _context.Quotations
                .Include(q => q.IdCostumerNavigation)
                .FirstOrDefaultAsync(m => m.IdQuotation == id);
            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }

        // POST: Quotation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quotations == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.Quotations'  is null.");
            }
            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation != null)
            {
                _context.Quotations.Remove(quotation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuotationExists(int id)
        {
          return (_context.Quotations?.Any(e => e.IdQuotation == id)).GetValueOrDefault();
        }
    }
}
