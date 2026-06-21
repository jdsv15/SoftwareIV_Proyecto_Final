using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [MaxLength(20)]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public DateTime? FechaUltimaAtencion { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string? UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public ApplicationUser? Usuario { get; set; }

        public string? MedicoId { get; set; }

        [ForeignKey(nameof(MedicoId))]
        public ApplicationUser? Medico { get; set; }
    }
}