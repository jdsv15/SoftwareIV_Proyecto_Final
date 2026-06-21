using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class PacienteTratamiento
    {
        [Key]
        public int Id { get; set; }

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        public int TratamientoId { get; set; }
        [ForeignKey("TratamientoId")]
        public Tratamiento Tratamiento { get; set; }

        public DateTime FechaAsignacion { get; set; }
        public bool Activo { get; set; } = true;
        public string MedicoId { get; set; }
    }
}