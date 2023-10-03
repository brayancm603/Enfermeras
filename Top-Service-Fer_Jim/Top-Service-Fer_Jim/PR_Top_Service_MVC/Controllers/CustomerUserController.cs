using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Controllers
{

    public class CustomerUserController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public CustomerUserController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: CustomerUser
        public async Task<IActionResult> Index()
        {
            return _context.CustomerUser != null ?
                        View(await _context.CustomerUser.ToListAsync()) :
                        Problem("Entity set 'TopServiceBDOContext.CustomerUser'  is null.");
        }

        // GET: CustomerUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerUser == null)
            {
                return NotFound();
            }

            var customerUser = await _context.CustomerUser
                .FirstOrDefaultAsync(m => m.IdCostumer == id);
            if (customerUser == null)
            {
                return NotFound();
            }



            return View(customerUser);
        }

        // GET: CustomerUser/Create
        public IActionResult Create()
        {

            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name");
            return View();
        }
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
                string hashedPassword = Convert.ToBase64String(hashedBytes);
                return hashedPassword;
            }
        }

        // POST: CustomerUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCostumer,Address,Ci,IdPerson,Name,LastName,SecondLastName,IdDepartment,status,IdUser,Email,Password,Role")] CustomerUser customerUser)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Person p = new()
                {
                    Name = customerUser.Name,
                    LastName = customerUser.LastName,
                    SecondLastName = customerUser.SecondLastName,
                    IdDepartment = customerUser.IdDepartment,
                    status = customerUser.status
                };
                _context.Add(p);
                await _context.SaveChangesAsync();
                Costumer c = new()
                {
                    IdCostumer = p.IdPerson,
                    Address = customerUser.Address,
                    Ci = customerUser.Ci
                };
                _context.Add(c);
                await _context.SaveChangesAsync();

                string Password = HashPassword(Request.Form["password"]);
                User u = new()
                {
                    IdUser = p.IdPerson,
                    Email = customerUser.Email,
                    Password = Password,
                    Role = customerUser.Role
                };
                _context.Add(u);
                await _context.SaveChangesAsync();



                ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", customerUser.IdDepartment);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "Ocurrió un error al Registrarse. Por favor, inténtalo nuevamente.");
                ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name");

                return View(customerUser);
            }




        }

        // GET: CustomerUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }




            ViewBag.person = await _context.People.FindAsync(id);
            ViewBag.user = await _context.Users.FindAsync(id);
            ViewBag.costumer = await _context.Costumers.FindAsync(id);
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", ViewBag.person.IdDepartment);

            return View();
        }

        // POST: CustomerUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCostumer,Address,Ci,IdPerson,Name,LastName,SecondLastName,IdDepartment,status,IdUser,Email,Password,Role")] CustomerUser customerUser)
        {


            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var person = await _context.People.FindAsync(id);
                var user = await _context.Users.FindAsync(id);
                var costumer = await _context.Costumers.FindAsync(id);

                if (person == null || user == null || costumer == null)
                {
                    return NotFound();
                }

                if (customerUser.Name != null)
                {
                    person.Name = customerUser.Name;
                }

                if (customerUser.LastName != null)
                {
                    person.LastName = customerUser.LastName;
                }

                if (customerUser.SecondLastName != null)
                {
                    person.SecondLastName = customerUser.SecondLastName;
                }

                if (customerUser.IdDepartment != null)
                {
                    person.IdDepartment = customerUser.IdDepartment;
                }

                if (customerUser.Email != null)
                {
                    user.Email = customerUser.Email;
                }

                if (customerUser.Password != null)
                {
                    user.Password = customerUser.Password;
                }

                if (customerUser.Role != null)
                {
                    user.Role = customerUser.Role;
                }

                if (customerUser.Address != null)
                {
                    costumer.Address = customerUser.Address;
                }

                if (customerUser.Ci != null)
                {
                    costumer.Ci = customerUser.Ci;
                }

                _context.Update(person);
                _context.Update(user);
                _context.Update(costumer);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();

                if (!CustomerUserExists(customerUser.IdCostumer))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la persona administrativa. Por favor, inténtalo nuevamente.");
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        // GET: CustomerUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerUser == null)
            {
                return NotFound();
            }

            var customerUser = await _context.CustomerUser
                .FirstOrDefaultAsync(m => m.IdCostumer == id);
            if (customerUser == null)
            {
                return NotFound();
            }

            return View(customerUser);
        }

        // POST: CustomerUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerUser == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.CustomerUser'  is null.");
            }
            var customerUser = await _context.CustomerUser.FindAsync(id);
            if (customerUser != null)
            {
                _context.CustomerUser.Remove(customerUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerUserExists(int id)
        {
            return (_context.CustomerUser?.Any(e => e.IdCostumer == id)).GetValueOrDefault();
        }
    }
}