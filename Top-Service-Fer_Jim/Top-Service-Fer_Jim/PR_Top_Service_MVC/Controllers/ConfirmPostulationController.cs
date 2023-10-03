using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;
using System.Diagnostics;

namespace PR_Top_Service_MVC.Controllers
{
    public class ConfirmPostulationController : Controller
    {
        private readonly TopServiceBDOContext _context;
        public ConfirmPostulationController(TopServiceBDOContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var postulation = _context.Postulations.Where(x => x.Status == 1);
            return View(postulation.ToList());
        }


        public IActionResult ViewPostulation(int id)
        {
            IQueryable<Postulation> person = from Postulation in _context.Postulations.Include(a => a.IdProfessionalNavigation).Include(a => a.IdProfessionalNavigation.IdProfesionalNavigation)
                                             join Profesional in _context.Profesionals on Postulation.IdProfessional equals Profesional.IdProfesional
                                             join Person in _context.People on Profesional.IdProfesional equals Person.IdPerson
                                             where Postulation.Status == 1 && Postulation.IdPostulation == id
                                             select Postulation;


            return View(person);
        }




        public async Task<IActionResult> otro(int id)
        {

            Debug.WriteLine("\n\n" + id + "\n\n");

            var postulation = _context.Postulations.First(x => x.IdPostulation == id);
            postulation.Status = 2;
            _context.Update(postulation);
            await _context.SaveChangesAsync();
            var user = _context.Users.First(x => x.IdUser == postulation.IdProfessional);
            user.Role = "Professional";
            _context.Update(user);
            await _context.SaveChangesAsync();
            var person = _context.People.First(x => x.IdPerson == postulation.IdProfessional);
            person.status = 1;
            _context.Update(person);
            await _context.SaveChangesAsync();
            AreaProfesional areaprof = new()
            {
                idArea = postulation.idArea,
                IdProfesional = postulation.IdProfessional
            };
            _context.Add(areaprof);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "ConfirmPostulation");
        }
    }
}
