using System;
using System.ComponentModel.DataAnnotations;

namespace SpaWebApp.Models
{
    public class Turno
    {
        public int TurnoID { get; set; }

        [Required]
        public string Servicio { get; set; } // Guardar el nombre del servicio

        [Required]
        public DateTime FechaTurno { get; set; }

        public string Estado { get; set; } = "Pendiente"; // Valor predeterminado

        public string Comentarios { get; set; } = string.Empty;

        [Required]
        public int UsuarioID { get; set; } // ID del usuario
        public Usuario? Usuario { get; set; } // Permitir que Usuario sea nullable
    }
}
