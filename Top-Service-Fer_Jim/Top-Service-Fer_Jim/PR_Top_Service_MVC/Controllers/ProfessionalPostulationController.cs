using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]

        public async Task<IActionResult> CreateProfessionalPostulation(ProfessionalPostulation p, IFormFile image)
        {
            Stream imagen = image.OpenReadStream();
            string urlImagen = await SubirStorage(imagen, image.FileName);
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == p.Email);
                if (existingUser != null)
                {
                    // El correo ya está en uso, mostrar un mensaje de error
                    ModelState.AddModelError("Email", "El correo electrónico ya está en uso");
                    // Luego, regresar a la vista de registro
                    ViewData["idArea"] = new SelectList(_context.JobArea, "idArea", "Name", p.idArea);
                    ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", p.IdDepartment);
                    return View("Index"); // Muestra la vista de registro con el mensaje de error
                }
                else
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
                    pos.Image = urlImagen;
                    pos.Date = p.Date;
                    pos.Address = p.Address;
                    pos.Status = 1;
                    pos.IdProfessional = per.IdPerson;
                    pos.Certification = p.Certification;
                    pos.ProfessionalTitles = p.ProfessionalTitles;
                    pos.idArea = p.idArea;
                    _context.Add(pos);
                    await _context.SaveChangesAsync();

                    Models.User user = new Models.User();
                    user.IdUser = per.IdPerson;
                    user.Email = p.Email;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    ViewData["idArea"] = new SelectList(_context.JobArea, "idArea", "Name", p.idArea);
                    ViewData["IdDepartment"] = new SelectList(_context.Departments, "IdDepartment", "Name", p.IdDepartment);
                    return RedirectToAction("Index", "Home");
                }

               

            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                return RedirectToAction("Error", "Home");

            }
        }

        public async Task<string> SubirStorage(Stream archivo, string nombre)
        {
            string email = "uunivalle2023@gmail.com";
            string clave = "univall32023";
            string ruta = "top-services-f7124.appspot.com";
            string api_key = "AIzaSyDQDDTLdzu6drgs1t76iOukBK6qesTEDiE";

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await authProvider.SignInWithEmailAndPasswordAsync(email, clave);

            var authToken = a.FirebaseToken;

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authToken),
                    ThrowOnCancel = true
                }
                )
                .Child("Fotos_CV").Child(nombre).PutAsync(archivo, cancellation.Token);
            var dowloadURL = await task;

            return dowloadURL;
        }
    }
}
