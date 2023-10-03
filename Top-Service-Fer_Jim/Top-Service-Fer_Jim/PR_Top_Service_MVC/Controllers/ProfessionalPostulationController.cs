using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Controllers
{
    public class ProfessionalPostulationController : Controller
    {

        private readonly TopServiceBDOContext _context;
        public ProfessionalPostulationController(TopServiceBDOContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            ViewData["idArea"] = new SelectList(_context.JobArea, "idArea", "Name");
            ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name");
            return View();
        }

        public async Task<IActionResult> CreateProfessionalPostulation(ProfessionalPostulation p)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
               
                Person per = new Person();
                per.Name = p.Name;
                per.LastName = p.LastName;
                per.SecondLastName = p.SecondLastName;
                per.IdDepartment = p.IdDepartment;
                per.status = 0;
                per.deleted = 0;
                _context.Add(per);
                await _context.SaveChangesAsync();

                Profesional prof = new Profesional();
                prof.IdProfesional = per.IdPerson;
                prof.Birthdate = p.Birthdate;
                _context.Add(prof);
                await _context.SaveChangesAsync();

                Postulation pos = new Postulation();
                pos.WorkExperience = p.WorkExperience;
                pos.Date = p.Date;
                pos.Address = p.Address;
                pos.Status = 1;
                pos.IdProfessional = per.IdPerson;
                pos.Certification = p.Certification;
                pos.ProfessionalTitles = p.ProfessionalTitles;
                pos.idArea = p.idArea;
                _context.Add(pos);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                ViewData["idArea"] = new SelectList(_context.JobArea, "idArea", "Name", p.idArea);
                ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", p.IdDepartment);
                return RedirectToAction("Index", "Home");

            }
            catch(Exception ex)
            {

                await transaction.RollbackAsync();
                return RedirectToAction("Error","Home");
               
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
