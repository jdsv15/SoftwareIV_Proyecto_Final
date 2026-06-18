using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class Medico
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de colegiado es obligatorio")]
        [Display(Name = "Número de Colegiado")]
        public string NumeroColegiado { get; set; } = string.Empty;

        public string? Fotografia { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public virtual ICollection<Especialidad> Especialidades { get; set; } = new List<Especialidad>();
    }
}