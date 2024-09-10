using System;
using System.ComponentModel.DataAnnotations;

namespace SpaWebApp.Models
{
    public class Comentario
    {
        public int ComentarioID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public string ComentarioTexto { get; set; }

        public DateTime FechaComentario { get; set; } = DateTime.Now;
    }
}
