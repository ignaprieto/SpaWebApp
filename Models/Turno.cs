using System;
using System.ComponentModel.DataAnnotations;

namespace SpaWebApp.Models
{
    public class Turno
    {
        public int TurnoID { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Servicio { get; set; }

        [Required]
        public DateTime FechaTurno { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public string Comentarios { get; set; }
    }
}
