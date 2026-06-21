using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class PacienteMedicamento
    {
        [Key]
        public int Id { get; set; }

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        public int MedicamentoId { get; set; }
        [ForeignKey("MedicamentoId")]
        public Medicamento Medicamento { get; set; }

        public DateTime FechaAsignacion { get; set; }
        public bool Activo { get; set; } = true;
        public string MedicoId { get; set; }
    }
}