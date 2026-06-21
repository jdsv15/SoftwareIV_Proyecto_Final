using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class PacientePadecimiento
    {
        [Key]
        public int Id { get; set; }

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        public int PadecimientoId { get; set; }
        [ForeignKey("PadecimientoId")]
        public Padecimiento Padecimiento { get; set; }

        public DateTime FechaDiagnostico { get; set; }
        public bool Activo { get; set; } = true; 
        public string MedicoId { get; set; }
    }
}