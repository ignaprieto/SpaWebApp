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
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Cliente")
                {
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                    if (userEmail != null)
                    {
                        var usuario = _context.Usuarios.SingleOrDefault(u => u.Email == userEmail);
                        if (usuario != null)
                        {
                            turno.UsuarioID = usuario.UsuarioID;

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

                _context.Turnos.Add(turno);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

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
        public IActionResult Eliminar([FromBody] EliminarTurnoRequest request)
        {
            var turno = _context.Turnos.Find(request.Id);
            if (turno == null)
            {
                return Json(new { success = false, message = "No se encontró el turno." });
            }

            _context.Turnos.Remove(turno);
            _context.SaveChanges();

            return Json(new { success = true, message = "Turno eliminado exitosamente." });
        }

        public class EliminarTurnoRequest
        {
            public int Id { get; set; }
        }
    }
}
