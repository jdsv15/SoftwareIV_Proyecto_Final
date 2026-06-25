using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class HistorialClinico
    {
        [Key]
        public int Id { get; set; }

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        public string MedicoId { get; set; } 

        public DateTime FechaHora { get; set; }

        [Required]
        public string Notas { get; set; }
    }
}