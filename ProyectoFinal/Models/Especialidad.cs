using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Especialidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Especialidad")]
        public string Nombre { get; set; } = string.Empty;

        public ICollection<Medico>? Medicos { get; set; }
    }
}