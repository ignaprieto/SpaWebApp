using Microsoft.AspNetCore.Mvc;

namespace SpaWebApp.Controllers
{
    public class ServiciosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
