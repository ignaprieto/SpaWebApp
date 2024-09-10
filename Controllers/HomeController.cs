using Microsoft.AspNetCore.Mvc;
using SpaWebApp.Data;
using SpaWebApp.Models;

namespace SpaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly SpaContext _context;

        public HomeController(SpaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                _context.Consultas.Add(consulta);
                _context.SaveChanges();
                ViewBag.Message = "Consulta enviada correctamente.";
            }

            return View("Index");
        }
    }
}
