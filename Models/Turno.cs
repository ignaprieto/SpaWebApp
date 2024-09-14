using System;
using System.ComponentModel.DataAnnotations;

namespace SpaWebApp.Models
{
    public enum ServicioEnum
    {
        AntiStress,
        Descontracturantes,
        PiedrasCalientes,
        Circulatorios,
        LiftingPestañas,
        DepilacionFacial,
        BellezaManosPies,
        PuntaDiamante,
        LimpiezaProfunda,
        CrioFrecuenciaFacial,
        VelaSlim,
        DermoHealth,
        CrioFrecuenciaCorporal,
        Ultracavitacion
    }

    public class Turno
    {
        public int TurnoID { get; set; }

        [Required]
        public ServicioEnum Servicio { get; set; } // Servicio específico

        [Required]
        public DateTime FechaTurno { get; set; }

        public string Estado { get; set; } = "Pendiente"; // Valor predeterminado

        public string Comentarios { get; set; } = string.Empty;

        [Required]
        public int UsuarioID { get; set; } // ID del usuario
        public Usuario? Usuario { get; set; } // Permitir que Usuario sea nullable
    }
}
