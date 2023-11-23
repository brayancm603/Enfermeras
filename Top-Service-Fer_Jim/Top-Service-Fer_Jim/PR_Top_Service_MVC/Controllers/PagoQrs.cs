using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR_Top_Service_MVC.Models;

namespace PR_Top_Service_MVC.Controllers
{
    [Authorize]
    public class PagoQrs : Controller
    {
        private readonly TopServiceBDOContext _context;
        public PagoQrs(TopServiceBDOContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ultimoPagoQr = _context.PagoQRs.OrderByDescending(p => p.id).FirstOrDefault();
            return View(ultimoPagoQr);
        }
        public IActionResult Detail(int id)
        {
            var totalA = _context.Receipts.Where(x => x.IdReceipt == id);
            ViewBag.Total = totalA.First().Total.ToString();
            var pagoQr = _context.PagoQRs.OrderByDescending(p => p.id).FirstOrDefault();
            return View(pagoQr);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cargar(PagoQr pagoQr, IFormFile image)
        {
            Stream imagen = image.OpenReadStream();
            string urlImagen = await SubirStorage(imagen, image.FileName);
            pagoQr.ImageQr = urlImagen;
            _context.Add(pagoQr);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = pagoQr.id });
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
                .Child("Fotos_QR").Child(nombre).PutAsync(archivo, cancellation.Token);
            var dowloadURL = await task;

            return dowloadURL;
        }
    }
}
