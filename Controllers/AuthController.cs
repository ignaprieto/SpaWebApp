using Microsoft.AspNetCore.Mvc;
using SpaWebApp.Data;
using SpaWebApp.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SpaWebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly SpaContext _context;

        public AuthController(SpaContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Buscar al usuario por email
            var user = _context.Usuarios.SingleOrDefault(u => u.Email == email);

            if (user == null || user.ContraseñaHash != password) // Validación básica de contraseña
            {
                // En caso de fallo
                ModelState.AddModelError("", "Email o contraseña incorrectos.");
                return View();
            }

            // Guardar información en sesión
            HttpContext.Session.SetString("UserId", user.UsuarioID.ToString());
            HttpContext.Session.SetString("UserName", user.Nombre);
            HttpContext.Session.SetString("UserRole", user.Rol);

            // Redireccionar dependiendo del rol
            if (user.Rol == "Personal")
            {
                return RedirectToAction("Dashboard", "Admin"); // Redirigir al panel de personal
            }
            return RedirectToAction("Index", "Home"); // Redirigir al área de clientes
        }

        // GET: Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(string Nombre, string Apellido, string Email, string Telefono, string Direccion, string Password)
        {
            // Verificar si ya existe un usuario con el mismo email
            var existingUser = _context.Usuarios.SingleOrDefault(u => u.Email == Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Ya existe un usuario con ese email.");
                return View();
            }

            // Crear un nuevo usuario
            var newUser = new Usuario
            {
                Nombre = Nombre,
                Apellido = Apellido,
                Email = Email,
                Telefono = Telefono,
                Direccion = Direccion,
                FechaRegistro = DateTime.Now,
                ContraseñaHash = Password, // Guardar la contraseña de forma directa (sin hashing)
                Rol = "Cliente" // Asignar el rol de cliente por defecto
            };

            // Agregar el nuevo usuario a la base de datos
            _context.Usuarios.Add(newUser);
            await _context.SaveChangesAsync();

            // Redireccionar al login tras el registro exitoso
            return RedirectToAction("Login", "Auth");
        }
    }
}
