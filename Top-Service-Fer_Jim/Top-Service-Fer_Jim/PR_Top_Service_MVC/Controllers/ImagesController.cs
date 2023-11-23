using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Controllers
{
    public class ImagesController : Controller
    {
        private readonly TopServiceBDOContext _context;

        public ImagesController(TopServiceBDOContext context)
        {
            _context = context;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            var topServiceContext = _context.Images.Include(i => i.IdServiceNavigation);
            return View(await topServiceContext.ToListAsync());
        }
        //GET: Images by Service ID
        public async Task<IActionResult> DetailsImage(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no fue proporcionado.");
            }

            var images = await _context.Images
                                       .Include(i => i.IdServiceNavigation)
                                       .Where(i => i.IdService == id)
                                       .ToListAsync();

            if (images == null || images.Count == 0)
            {
                TempData["ErrorMessage"] = "No se encontraron comprobantes para este servicio.";
                return RedirectToAction("ServiceFinalized", "Service");
            }

            return View(images);
        }

        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.IdServiceNavigation)
                .FirstOrDefaultAsync(m => m.IdImages == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        int idCreate = 0;
        // GET: Images/Create
        public IActionResult Create(int id)
        {
            idCreate = id;
            var service = _context.Services.FirstOrDefault(s => s.IdService == id);

            if (service != null)
            {
                // Obtén la descripción del servicio
                string description = service.Description;

                // Luego, puedes usar la descripción para establecer el ViewData
                ViewData["IdService"] = description;
                ViewData["Id"] = id;
            }
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Image image, List<IFormFile> ImageService, int IdService)
        {

            // Asigna el IdService
            image.IdService = IdService;


                foreach (var file in ImageService)
                {
                    Stream imagen = file.OpenReadStream();
                    string urlImagen = await SubirStorage(imagen, file.FileName);

                    // Crea un nuevo objeto Image para cada imagen
                    var imageEntity = new Image
                    {
                        IdService = image.IdService,
                        Imaged = urlImagen
                    };

                    // Agrega la imagen a la base de datos
                    _context.Images.Add(imageEntity);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Calendar", new { id = UserConfig.userLogin.IdUser });
            

        }

        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            ViewData["IdService"] = new SelectList(_context.Services, "IdService", "IdService", image.IdService);
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdImages,IdService,Imaged")] Image image)
        {
            if (id != image.IdImages)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.IdImages))
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
            ViewData["IdService"] = new SelectList(_context.Services, "IdService", "IdService", image.IdService);
            return View(image);
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .Include(i => i.IdServiceNavigation)
                .FirstOrDefaultAsync(m => m.IdImages == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Images == null)
            {
                return Problem("Entity set 'TopServiceContext.Images'  is null.");
            }
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                _context.Images.Remove(image);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return (_context.Images?.Any(e => e.IdImages == id)).GetValueOrDefault();
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
                .Child("Fotos_Servicio").Child(nombre).PutAsync(archivo, cancellation.Token);
            var dowloadURL = await task;

            return dowloadURL;
        }
    }
}
