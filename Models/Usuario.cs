using System;
using System.ComponentModel.DataAnnotations;

namespace SpaWebApp.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        public string ContraseñaHash { get; set; }

        [Required]
        [StringLength(50)]
        public string Rol { get; set; }
    }
}
