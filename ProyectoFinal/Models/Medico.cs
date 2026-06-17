using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class Medico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Número de Colegiado")]
        public string NumeroColegiado { get; set; } = string.Empty;

        public string? Fotografia { get; set; }

        [Required]
        public int EspecialidadId { get; set; }
        [ForeignKey("EspecialidadId")]
        public Especialidad? Especialidad { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; } 
    }
}