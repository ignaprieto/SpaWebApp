using System;
using System.ComponentModel.DataAnnotations;

namespace SpaWebApp.Models
{
    public class Turno
    {
        public int TurnoID { get; set; } // Identificador del turno

        // Cliente que reserva el turno
        [Required]
        public int UsuarioID { get; set; } // ID del cliente
        public Usuario Usuario { get; set; } // Navegación para el cliente

        // Profesional que atiende el turno
        public int? ProfesionalID { get; set; }  // ID del profesional (nullable, en caso de que no esté asignado)
        public Usuario Profesional { get; set; } // Navegación para el profesional

        // Detalles del servicio
        [Required]
        public string Servicio { get; set; } // Guardar el nombre del servicio

        // Fecha y hora del turno
        [Required]
        public DateTime FechaTurno { get; set; } // Fecha del turno

        // Estado del turno
        public string Estado { get; set; } = "Pendiente"; // Valor predeterminado

        // Comentarios adicionales sobre el turno
        public string Comentarios { get; set; } = string.Empty; // Valor predeterminado vacío
    }
}
