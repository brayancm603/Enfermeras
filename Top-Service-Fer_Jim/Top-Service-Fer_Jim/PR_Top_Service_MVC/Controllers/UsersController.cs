﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Security.Cryptography;


namespace PR_Top_Service_MVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public UsersController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var topServiceBDOContext = _context.Users.Include(u => u.IdUserNavigation);
            return View(await topServiceBDOContext.ToListAsync());
        }

        public async Task<IActionResult> Profile(int id)
        {
            IQueryable<Person> person = from Person in _context.People.Include(a => a.IdDepartmentNavigation).Include(a => a.User)
                                        join Department in _context.Departments on Person.IdDepartment equals Department.IdDepartment
                                        where Person.IdPerson == id
                                        select Person;



            return View(await person.ToListAsync());
        }

        public async Task<IActionResult> Profile1(int id)
        {
            IQueryable<Postulation> person = from Postulation in _context.Postulations.Include(a => a.IdProfessionalNavigation).Include(a => a.IdProfessionalNavigation.IdPersonNavigation)
                                             join Profesional in _context.Profesionals on Postulation.IdProfessional equals Profesional.IdProfesional
                                             join Person in _context.People on Profesional.IdProfesional equals Person.IdPerson
                                             where Postulation.Status == 1 && Postulation.IdPostulation == id
                                             select Postulation;



            return View(await person.ToListAsync());
        }
        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["IdUser"] = new SelectList(_context.People, "IdPerson", "IdPerson");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUser,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUser"] = new SelectList(_context.People, "IdPerson", "IdPerson", user.IdUser);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.People, "IdPerson", "IdPerson", user.IdUser);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUser,Email,Password,Role")] User user)
        {
            if (id != user.IdUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.IdUser))
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
            ViewData["IdUser"] = new SelectList(_context.People, "IdPerson", "IdPerson", user.IdUser);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.IdUserNavigation)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'TopServiceBDOContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.IdUser == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> UpdatePassword(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["IdUser"] = new SelectList(_context.People, "IdPerson", "IdPerson", user.IdUser);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(int id, [Bind("IdUser,Email,Password,Role")] User user)
        {
            if (id == user.IdUser)
            {
                try
                {
                    string password = HashPassword(user.Password);
                    user.Email = UserConfig.userLogin.Email;
                    user.Password = password;
                    user.Role = UserConfig.userLogin.Role;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.IdUser))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(user);
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

    }

}
