using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaWebApp.Models;
using SpaWebApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SpaWebApp.Controllers
{
    [Authorize]
    public class TurnosController : Controller
    {
        private readonly SpaContext _context;

        public TurnosController(SpaContext context)
        {
            _context = context;
        }

        // GET: Turnos
        public IActionResult Index()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (userRole == "Personal")
            {
                var turnos = _context.Turnos
                    .Include(t => t.Usuario)
                    .ToList();
                return View("IndexPersonal", turnos); // Vista para el personal
            }
            else
            {
                return RedirectToAction("Reservar");
            }
        }

        // GET: Turnos/Reservar
        public IActionResult Reservar()
        {
            // Eliminar ViewBag.TiposServicio ya que no se usa más
            return View();
        }

        // POST: Turnos/Reservar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reservar(Turno turno)
        {
            if (ModelState.IsValid)
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Cliente")
                {
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                    if (userEmail != null)
                    {
                        var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == userEmail);
                        if (usuario != null)
                        {
                            turno.UsuarioID = usuario.UsuarioID; // Asignar solo el ID del usuario
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "No se encontró el usuario.");
                            return View(turno);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se encontró el email del usuario.");
                        return View(turno);
                    }
                }
                else
                {
                    // Manejar el caso en que el rol no es "Cliente"
                    ModelState.AddModelError(string.Empty, "No tienes permiso para reservar turnos.");
                    return View(turno);
                }

                // Guardar el turno en la base de datos
                _context.Turnos.Add(turno);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // No hay necesidad de cargar tipos de servicio ya que no se usa más
            return View(turno);
        }
    }
}
