using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PR_Top_Service_MVC.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;

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
            IQueryable<Postulation> person = from Postulation in _context.Postulations.Include(a => a.IdProfessionalNavigation).Include(a => a.IdProfessionalNavigation.IdPersonNavigation)
                                             join Profesional in _context.Profesionals on Postulation.IdProfessional equals Profesional.IdProfesional
                                             join Person in _context.People on Profesional.IdProfesional equals Person.IdPerson
                                             where Postulation.Status == 1 && Postulation.IdPostulation == id
                                             select Postulation;


            return View(person);
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
        public async Task<IActionResult> otro(int id)
        {
            Debug.WriteLine("\n\n" + id + "\n\n");

            var postulation = _context.Postulations.FirstOrDefault(x => x.IdPostulation == id);

            if (postulation != null)
            {
                postulation.Status = 2;
                _context.Update(postulation);
                await _context.SaveChangesAsync();

                var user = _context.Users.FirstOrDefault(x => x.IdUser == postulation.IdProfessional);
                string pass = GeneratePass();
                SendGmail(user.Email, pass);
                string Password = HashPassword(pass);
                if (user != null)
                {
                    user.Password = Password;
                    user.Role = "Professional";
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                var person = _context.People.FirstOrDefault(x => x.IdPerson == postulation.IdProfessional);
                if (person != null)
                {
                    person.status = 1;
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }

                AreaProfesional areaprof = new()
                {
                    idArea = postulation.idArea,
                    IdProfesional = postulation.IdProfessional
                };
                _context.Add(areaprof);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "ConfirmPostulation");
        }

        public async void SendGmail(string email, string pass)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com",587); // Reemplaza "smtp.example.com" con tu servidor SMTP
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("jperezlopez7895@gmail.com", "xzzzbjpxknkojzvu"); // Cambia estos valores
                smtpClient.EnableSsl = true; 

                // Crea el mensaje de correo electrónico
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("jperezlopez7895@gmail.com"); // Dirección de correo electrónico del remitente
                mailMessage.To.Add(email); // Dirección de correo electrónico del destinatario
                mailMessage.Subject = "Bienvenido"; // Asunto del correo
                mailMessage.Body = @"Te damos la bienvenida a nuestra comunidad de Top Services"+
                                    "Inica sesion con esta contraseña " + pass +
                                    "Por favor cambie la contraseña una vez ingresado"; // Cuerpo del correo

                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine("Correo electrónico enviado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo electrónico: " + ex.Message);
            }

        }

        public string GeneratePass()
        {
            int cod = new Random().Next(1000, 99999);
            string password = cod.ToString();
            return password;
        }
    }
}
