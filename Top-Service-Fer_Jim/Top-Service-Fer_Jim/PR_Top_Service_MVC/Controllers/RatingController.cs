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
    public class RatingController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public RatingController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: JobArea
        public async Task<IActionResult> Index()
        {
            return _context.Ratings != null ?
                        View(await _context.Ratings.ToListAsync()) :
                        Problem("Entity set 'TopServiceBDOContext.JobArea'  is null.");
        }


        public async Task<IActionResult> Comments(int id)
        {
            return _context.Ratings != null ?
                        View(await _context.Ratings.Where(x => x.IdProfesional == id).ToListAsync()) :
                        Problem("Entity set 'TopServiceBDOContext.JobArea'  is null.");
        }
        public async Task<IActionResult> Ranking()
        {
            var consulta = from rating in _context.Ratings
                           join profesional in _context.Profesionals on rating.IdProfesional equals profesional.IdProfesional
                           join persona in _context.People on profesional.IdProfesional equals persona.IdPerson
                           where persona.IdDepartment == UserConfig.DepPer
                           group rating by new
                           {
                               ProfesionalId = profesional.IdProfesional,
                               Nombre = persona.Name,
                               Apellido = persona.LastName
                           } into grp
                           select new Ranking
                           {
                               IdProfessional = grp.Key.ProfesionalId,
                               Nombre = grp.Key.Nombre,
                               Apellido = grp.Key.Apellido,
                               PromedioRating = grp.Average(r => r.rating)
                           } into resultado
                           orderby resultado.PromedioRating descending
                           
                           select resultado;

            var resultados = await consulta.ToListAsync();

            return View("Ranking", resultados);
        }

        // GET: JobArea/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var jobArea = await _context.Ratings
                .FirstOrDefaultAsync(m => m.IdRating == id);
            if (jobArea == null)
            {
                return NotFound();
            }

            return View(jobArea);
        }

        // GET: JobArea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobArea/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("IdRating,rating,IdCostumer,IdProfesional,Description")] Rating comment)
        {
            if (ModelState.IsValid)
            {
                int punctuation = int.Parse(Request.Form["rating"]);
                string description = Request.Form["Description"];
                var Service = await _context.Services.FindAsync(id);
                comment.IdProfesional = Service.IdProfessional;
                comment.rating = punctuation;
                comment.IdCostumer = UserConfig.userLogin.IdUser;
                comment.Description = description;
                _context.Add(comment);
                await _context.SaveChangesAsync();


                Service.IdService = id;
                Service.IdAdmin = Service.IdAdmin;
                Service.IdProfessional = Service.IdProfessional;
                Service.Description = Service.Description;
                Service.Date = Service.Date;
                Service.Status = Service.Status;
                Service.IdCostumer = Service.IdCostumer;
                Service.Status_Service = Service.Status_Service;
                Service.DateTime_On_Service = Service.DateTime_On_Service;
                Service.DateTime_Off_Service = Service.DateTime_Off_Service;
                Service.Latitude = Service.Latitude;
                Service.Longitude = Service.Longitude;
                Service.deleted = Service.deleted;
                Service.Commented = "Y";
                _context.Update(Service);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction("ServiceByClient", "Service");
        }

        // GET: JobArea/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var jobArea = await _context.Ratings.FindAsync(id);
            if (jobArea == null)
            {
                return NotFound();
            }
            return View(jobArea);
        }

        // POST: JobArea/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(byte id, Rating comment)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index", "JobArea");

            }

            return RedirectToAction("Index", "JobArea");
        }

        // GET: JobArea/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null || _context.Ratings == null)
            {
                return NotFound();
            }

            var jobArea = await _context.Ratings
                .FirstOrDefaultAsync(m => m.IdRating == id);
            if (jobArea == null)
            {
                return NotFound();
            }

            return View(jobArea);
        }

        // POST: JobArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {

            var jobArea = await _context.Ratings.FindAsync(id);
            if (jobArea != null)
            {

            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobAreaExists(byte id)
        {
            return (_context.JobArea?.Any(e => e.idArea == id)).GetValueOrDefault();
        }
    }
}
