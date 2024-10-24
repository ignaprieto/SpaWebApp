using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaWebApp.Models;
using SpaWebApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            // Obtener el email del usuario autenticado
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // Obtener el usuario de la base de datos
            var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == userEmail);

            if (usuario == null)
            {
                return RedirectToAction("Login", "Auth"); // Redirigir si el usuario no existe
            }

            // Verificar el rol del usuario
            switch (usuario.Rol)
            {
                case "Cliente":
                    return RedirectToAction("Reservar"); // Redirigir a la vista de reservas para clientes

                case "Profesional":
                    var turnosProfesional = _context.Turnos
                        .Include(t => t.Usuario)
                        .ToList();
                    return View("Index", turnosProfesional); // Vista de turnos para el profesional

                case "Administrador":
                    var turnosAdmin = _context.Turnos
                        .Include(t => t.Usuario)
                        .ToList();
                    return View("IndexPersonal", turnosAdmin); // Vista de turnos para el administrador

                default:
                    return RedirectToAction("Login", "Auth"); // Redirigir al login si no tiene un rol válido
            }
        }

        // GET: Turnos/Reservar
        public IActionResult Reservar()
        {
            var servicios = new List<SelectListItem>
            {
                new SelectListItem { Value = "Masajes_AntiStress", Text = "Masajes AntiStress" },
                new SelectListItem { Value = "Masajes_Descontracturantes", Text = "Masajes Descontracturantes" },
                new SelectListItem { Value = "Masajes_PiedrasCalientes", Text = "Masajes con Piedras Calientes" },
                new SelectListItem { Value = "Masajes_Circulatorios", Text = "Masajes Circulatorios" },
                new SelectListItem { Value = "Belleza_LiftingPestañas", Text = "Belleza: Lifting de Pestañas" },
                new SelectListItem { Value = "Belleza_DepilacionFacial", Text = "Belleza: Depilación Facial" },
                new SelectListItem { Value = "BellezaManosPies", Text = "Belleza de Manos y Pies" },
                new SelectListItem { Value = "Tratamientos_Faciales_PuntaDiamante", Text = "Tratamientos Faciales: Punta de Diamante" },
                new SelectListItem { Value = "Tratamientos_Faciales_LimpiezaProfunda", Text = "Tratamientos Faciales: Limpieza Profunda" },
                new SelectListItem { Value = "Tratamientos_Faciales_CrioFrecuenciaFacial", Text = "Tratamientos Faciales: Crio Frecuencia Facial" },
                new SelectListItem { Value = "Tratamientos_Corporales_VelaSlim", Text = "Tratamientos Corporales: VelaSlim" },
                new SelectListItem { Value = "Tratamientos_Corporales_DermoHealth", Text = "Tratamientos Corporales: DermoHealth" },
                new SelectListItem { Value = "Tratamientos_Corporales_CrioFrecuenciaCorporal", Text = "Tratamientos Corporales: Crio Frecuencia Corporal" },
                new SelectListItem { Value = "Tratamientos_Corporales_Ultracavitacion", Text = "Tratamientos Corporales: Ultracavitación" }
            };

            ViewBag.Servicios = servicios;

            return View();
        }

        // POST: Turnos/Reservar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reservar(Turno turno, string HorarioTurno)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == userEmail);

                if (usuario != null)
                {
                    turno.UsuarioID = usuario.UsuarioID; // Asignar ID del cliente

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

                // Guardar el turno en la base de datos
                _context.Turnos.Add(turno);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, vuelve a la vista con el modelo actual
            return View(turno);
        }

        // POST: ActualizarEstado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ActualizarEstado(Dictionary<int, string> estados)
        {
            foreach (var estado in estados)
            {
                var turno = _context.Turnos.SingleOrDefault(t => t.TurnoID == estado.Key);
                if (turno != null)
                {
                    turno.Estado = estado.Value;
                }
            }
            _context.SaveChanges();

            return Json(new { success = true });
        }

        // POST: Eliminar Turno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarTurno(int turnoID)
        {
            var turno = _context.Turnos.SingleOrDefault(t => t.TurnoID == turnoID);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
