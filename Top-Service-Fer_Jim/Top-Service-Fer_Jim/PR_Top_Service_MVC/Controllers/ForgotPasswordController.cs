using Microsoft.AspNetCore.Mvc;
using PR_Top_Service_MVC.Models;
using System.Web.Helpers;
using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;

namespace PR_Top_Service_MVC.Controllers
{
   
    public class ForgotPasswordController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public ForgotPasswordController(TopServiceBDOContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendMail(User u)
        {
            try
            {
                var newUs = from user in _context.Users
                            where user.Email == u.Email
                            select user;

                newUs.First().Password = "defautPassword";

                _context.Update(newUs.First());
                await _context.SaveChangesAsync();
            
                SmtpClient clienteSmtp = new SmtpClient("smtp.gmail.com", 587);
                clienteSmtp.EnableSsl = true;
                clienteSmtp.UseDefaultCredentials = false;
                clienteSmtp.Credentials = new NetworkCredential("teranjegre@gmail.com", "poxialgpbqgfjmpd");

                MailMessage mensaje = new MailMessage("teranjegre@gmail.com", u.Email , "Se olvido su contraña..", "su contraseña temporal es 'defautPassword'");
                clienteSmtp.Send(mensaje);
                return Redirect("/");
            }
            catch
            {
                return Redirect("/Home/Error");
            }

            return View();
        }
    }
}
