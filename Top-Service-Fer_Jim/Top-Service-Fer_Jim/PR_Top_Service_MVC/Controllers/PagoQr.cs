using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PR_Top_Service_MVC.Controllers
{
    [Authorize]
    public class PagoQr : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task< IActionResult> Cargar(IFormFile image)
        {
            string name = "CodigoQr";
            string path = "wwwroot/images/QrCode/"+ name+".jpg";
            

           
            using (Stream stream = new FileStream(path, FileMode.Create))
            { 
                await image.CopyToAsync(stream);
            }
            return RedirectToAction("Index" , "PagoQr");
        }
    }
}
