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
            // No se necesita cargar tipos de servicio
            return View();
        }

        // POST: Turnos/Reservar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reservar(Turno turno, string HorarioTurno) // Captura el horario desde el formulario
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

                            // Unificar fecha y hora del turno en el campo FechaTurno
                            if (DateTime.TryParse(turno.FechaTurno.ToString("yyyy-MM-dd") + " " + HorarioTurno, out DateTime fechaCompleta))
                            {
                                turno.FechaTurno = fechaCompleta;
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Error al procesar la fecha y la hora del turno.");
                                return View(turno);
                            }
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
                    ModelState.AddModelError(string.Empty, "No tienes permiso para reservar turnos.");
                    return View(turno);
                }

                // Guardar el turno en la base de datos
                _context.Turnos.Add(turno);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(turno);
        }
    }
}
