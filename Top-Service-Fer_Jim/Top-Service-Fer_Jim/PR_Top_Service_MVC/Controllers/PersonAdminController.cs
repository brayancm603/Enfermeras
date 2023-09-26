using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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

    [Authorize]
    public class PersonAdminController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public PersonAdminController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: PersonAdmin
        public async Task<IActionResult> Index()
        {
            using (TopServiceBDOContext db = new TopServiceBDOContext())
            {
                ViewBag.PersonAdminUser = (from u in db.Users
                                           join p in db.People
                                           on u.IdUser equals p.IdPerson
                                           join a in db.Admins
                                           on p.IdPerson equals a.IdAdmin
                                           where u.IdUser == p.IdPerson && a.IdAdmin == u.IdUser && p.IdPerson == a.IdAdmin && p.status == 1
                                           select new
                                           {
                                               idPAU = p.IdPerson,
                                               name = p.Name,
                                               lastName = p.LastName,
                                               secondLastName = p.SecondLastName,
                                               email = u.Email,
                                               cellPhoneNumber = a.CelphoneNumber,
                                               Birthdate = a.Birthdate
                                           }).ToList();
                return View();
            }
        }

        // GET: PersonAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PersonAdmin == null)
            {
                return NotFound();
            }

            var personAdmin = await _context.PersonAdmin
                .FirstOrDefaultAsync(m => m.IdAdmin == id);
            if (personAdmin == null)
            {
                return NotFound();
            }

            return View(personAdmin);
        }

        // GET: PersonAdmin/Create
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
        // POST: PersonAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAdmin,CelphoneNumber,Birthdate,deleted,IdPerson,Name,LastName,SecondLastName,IdDepartment,status,IdUser,Email,Password,Role")] PersonAdmin personAdmin)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (ModelState.IsValid)
                {
                    Person p = new()
                    {
                        Name = personAdmin.Name,
                        LastName = personAdmin.LastName,
                        SecondLastName = personAdmin.SecondLastName,
                        IdDepartment = personAdmin.IdDepartment,
                        status = 1,
                        deleted = 0
                    };
                    _context.Add(p);
                    await _context.SaveChangesAsync();

                    Admin a = new()
                    {
                        IdAdmin = p.IdPerson,
                        CelphoneNumber = personAdmin.CelphoneNumber,
                        Birthdate = personAdmin.Birthdate,
                    };

                    _context.Add(a);
                    await _context.SaveChangesAsync();

                    string Password = HashPassword(Request.Form["password"]);
                    User u = new()
                    {
                        IdUser = p.IdPerson,
                        Email = personAdmin.Email,
                        Password = Password,
                        Role = personAdmin.Role
                    };
                    SendEmail(u.Email, Request.Form["password"]);

                    _context.Add(u);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", personAdmin.IdDepartment);

                    return RedirectToAction("Index", "PersonAdmin");
                }
                else
                {
                    await transaction.RollbackAsync();

                    ModelState.AddModelError("", "Ocurrió un error al crear al admin. Por favor, inténtalo nuevamente.");
                    ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", personAdmin.IdDepartment);
                    return View(personAdmin);
                }
            }
            catch
            {
                await transaction.RollbackAsync();

                ModelState.AddModelError("", "Ocurrió un error al crear al admin. Por favor, inténtalo nuevamente.");
                ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", personAdmin.IdDepartment);
                return View(personAdmin);
            }
        }

        // GET: PersonAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }




            ViewBag.person = await _context.People.FindAsync(id);
            ViewBag.user = await _context.Users.FindAsync(id);
            ViewBag.admin = await _context.Admins.FindAsync(id);
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", ViewBag.person.IdDepartment);

            return View();
        }

        // POST: PersonAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAdmin,CelphoneNumber,Birthdate,deleted,,IdPerson,Name,LastName,SecondLastName,IdDepartment,status,IdUser,Email,Password,Role")] PersonAdmin personAdmin)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var person = await _context.People.FindAsync(id);
                var user = await _context.Users.FindAsync(id);
                var admin = await _context.Admins.FindAsync(id);

                if (person == null || user == null || admin == null)
                {
                    return NotFound();
                }

                if (personAdmin.Name != null)
                {
                    person.Name = personAdmin.Name;
                }

                if (personAdmin.LastName != null)
                {
                    person.LastName = personAdmin.LastName;
                }

                if (personAdmin.SecondLastName != null)
                {
                    person.SecondLastName = personAdmin.SecondLastName;
                }

                if (personAdmin.IdDepartment != null)
                {
                    person.IdDepartment = personAdmin.IdDepartment;
                }

                if (personAdmin.Email != null)
                {
                    user.Email = personAdmin.Email;
                }

                if (personAdmin.Password != null)
                {
                    user.Password = personAdmin.Password;
                }

                if (personAdmin.Role != null)
                {
                    user.Role = personAdmin.Role;
                }

                if (personAdmin.CelphoneNumber != null)
                {
                    admin.CelphoneNumber = personAdmin.CelphoneNumber;
                }

                if (personAdmin.Birthdate != null)
                {
                    admin.Birthdate = personAdmin.Birthdate;
                }

                _context.Update(person);
                _context.Update(user);
                _context.Update(admin);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();

                if (!PersonAdminExists(personAdmin.IdAdmin))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la persona administrativa. Por favor, inténtalo nuevamente.");
                    ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", ViewBag.person.IdDepartment);
                    return View(personAdmin);
                }
            }
        }

        // GET: PersonAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            ViewBag.person = await _context.People.FindAsync(id);
            ViewBag.user = await _context.Users.FindAsync(id);
            ViewBag.admin = await _context.Admins.FindAsync(id);

            return View();
        }

        // POST: PersonAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [Bind("IdAdmin,CelphoneNumber,Birthdate,IdPerson,Name,LastName,SecondLastName,IdDepartment,IdUser,Email,Password,Role")] PersonAdmin personAdmin)
        {
            try
            {
                var person = await _context.People.FindAsync(id);
                var user = await _context.Users.FindAsync(id);
                var admin = await _context.Admins.FindAsync(id);

                person.status = 0;

                _context.Update(person);
                _context.Update(user);
                _context.Update(admin);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonAdminExists(personAdmin.IdAdmin))
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

        private bool PersonAdminExists(int id)
        {
            return (_context.PersonAdmin?.Any(e => e.IdAdmin == id)).GetValueOrDefault();
        }
        public void SendEmail(string variable1, string variable2)
        {
            // Dirección de correo electrónico del remitente
            string fromEmail = "rolycgr12@gmail.com";
            // Contraseña del remitente
            string fromPassword = "acayipybfwplzuvd";

            // Dirección de correo electrónico del destinatario
            string toEmail = variable1;

            // Crear el mensaje de correo electrónico
            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);
            mailMessage.Subject = "Credenciales para el Inicio de Session en TopService";
            mailMessage.Body = $"Email : {variable1}<br/> Contraseña : {variable2}";
            mailMessage.IsBodyHtml = true;

            // Configurar el cliente SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromEmail, fromPassword);

            // Enviar el correo electrónico
            smtpClient.Send(mailMessage);
        }
    }
}