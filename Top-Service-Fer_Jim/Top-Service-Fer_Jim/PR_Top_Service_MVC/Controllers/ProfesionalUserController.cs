using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Controllers
{
    public class ProfesionalUserController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public ProfesionalUserController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: ProfesionalUser
        public async Task<IActionResult> Index()
        {
            using (TopServiceBDOContext db = new TopServiceBDOContext())
            {
                ViewBag.PersonProfUser = (from u in db.Users
                                          join p in db.People
                                          on u.IdUser equals p.IdPerson
                                          join a in db.Profesionals
                                          on p.IdPerson equals a.IdProfesional
                                          where u.IdUser == p.IdPerson && a.IdProfesional == u.IdUser && p.IdPerson == a.IdProfesional && p.status == 0 && p.deleted == 0
                                          select new
                                          {
                                              idPAU = p.IdPerson,
                                              name = p.Name,
                                              lastName = p.LastName,
                                              secondLastName = p.SecondLastName,
                                              email = u.Email,
                                              Birthdate = a.Birthdate
                                          }).ToList();
                return View();
            }
        }

        // GET: ProfesionalUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProfesionalUser == null)
            {
                return NotFound();
            }

            var profesionalUser = await _context.ProfesionalUser
                .FirstOrDefaultAsync(m => m.IdProfesional == id);
            if (profesionalUser == null)
            {
                return NotFound();
            }

            return View(profesionalUser);
        }

        // GET: ProfesionalUser/Create
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
        // POST: ProfesionalUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProfesional,Birthdate,IdPerson,Name,LastName,SecondLastName,IdDepartment,status,Deleted,IdUser,Email,Password,Role")] ProfesionalUser profesionalUser)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Person p = new()
                {
                    Name = profesionalUser.Name,
                    LastName = profesionalUser.LastName,
                    SecondLastName = profesionalUser.SecondLastName,
                    IdDepartment = profesionalUser.IdDepartment,
                    status = 0,
                    deleted = 0
                };
                _context.Add(p);
                await _context.SaveChangesAsync();
                Profesional pr = new()
                {
                    IdProfesional = p.IdPerson,
                    Birthdate = profesionalUser.Birthdate
                };
                _context.Add(pr);
                await _context.SaveChangesAsync();
                string Password = HashPassword(Request.Form["password"]);
                User u = new()
                {
                    IdUser = p.IdPerson,
                    Email = profesionalUser.Email,
                    Password = Password,
                    Role = "Postulation"
                };
                _context.Add(u);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                UserConfig.userLogin = u;
                ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", profesionalUser.IdDepartment);
                return RedirectToAction("Create", "Postulation");
            }
            catch
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError("", "Ocurrió un error al crear la persona administrativa. Por favor, inténtalo nuevamente.");
                ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", profesionalUser.IdDepartment);
                return View(profesionalUser);
            }

        }



        // GET: ProfesionalUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProfesionalUser == null)
            {
                return NotFound();
            }

            ViewBag.person = await _context.People.FindAsync(id);
            ViewBag.user = await _context.Users.FindAsync(id);
            ViewBag.profesional = await _context.Profesionals.FindAsync(id);
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", ViewBag.person.IdDepartment);
            return View();
        }

        // POST: ProfesionalUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProfesional,Birthdate,IdPerson,Name,LastName,SecondLastName,IdDepartment,status,Deleted,IdUser,Email,Password,Role")] ProfesionalUser profesionalUser)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var person = await _context.People.FindAsync(id);
                var user = await _context.Users.FindAsync(id);
                var profesional = await _context.Profesionals.FindAsync(id);



                if (person == null || user == null || profesional == null)
                {
                    return NotFound();
                }



                if (profesionalUser.Name != null)
                {
                    person.Name = profesionalUser.Name;
                }



                if (profesionalUser.LastName != null)
                {
                    person.LastName = profesionalUser.LastName;
                }



                if (profesionalUser.SecondLastName != null)
                {
                    person.SecondLastName = profesionalUser.SecondLastName;
                }



                if (profesionalUser.IdDepartment != null)
                {
                    person.IdDepartment = profesionalUser.IdDepartment;
                }



                if (profesionalUser.Email != null)
                {
                    user.Email = profesionalUser.Email;
                }



                if (profesionalUser.Password != null)
                {
                    user.Password = profesionalUser.Password;
                }



                if (profesionalUser.Role != null)
                {
                    user.Role = profesionalUser.Role;
                }



                if (profesionalUser.Birthdate != null)
                {
                    profesional.Birthdate = profesionalUser.Birthdate;
                }
                _context.Update(person);
                _context.Update(user);
                _context.Update(profesional);



                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", profesionalUser.IdDepartment);
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();



                ModelState.AddModelError("", "Ocurrió un error al crear la persona administrativa. Por favor, inténtalo nuevamente.");



                return View(profesionalUser);
            }

        }

        // GET: ProfesionalUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProfesionalUser == null)
            {
                return NotFound();
            }

            ViewBag.person = await _context.People.FindAsync(id);
            ViewBag.user = await _context.Users.FindAsync(id);
            ViewBag.profesional = await _context.Profesionals.FindAsync(id);

            return View();
        }

        // POST: ProfesionalUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("IdAdmin,CelphoneNumber,Birthdate,IdPerson,Name,LastName,SecondLastName,IdDepartment,IdUser,Email,Password,Role")] PersonAdmin personAdmin)
        {
            try
            {
                var person = await _context.People.FindAsync(id);
                var user = await _context.Users.FindAsync(id);
                var profecional = await _context.Admins.FindAsync(id);

                person.deleted = 1;

                _context.Update(person);
                _context.Update(user);
                _context.Update(profecional);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesionalUserExists(personAdmin.IdAdmin))
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

        private bool ProfesionalUserExists(int id)
        {
            return (_context.ProfesionalUser?.Any(e => e.IdProfesional == id)).GetValueOrDefault();
        }
    }
}
