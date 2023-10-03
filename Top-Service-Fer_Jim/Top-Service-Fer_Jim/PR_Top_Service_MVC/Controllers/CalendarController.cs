using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly TopServiceBDOContext _context;
        public CalendarController(TopServiceBDOContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            var services = _context.Services.Where(x => x.IdProfessional == UserConfig.userLogin.IdUser && x.Status == 1 || x.Status==2);



            return View(services.ToList());
        }
    }
}
