using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaWebApp.Models
{
    public class Consulta
    {
        public int ConsultaID { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public string Mensaje { get; set; }

        public DateTime FechaConsulta { get; set; } = DateTime.Now;

        public string Respuesta { get; set; }
        public DateTime? FechaRespuesta { get; set; }
    }
}
