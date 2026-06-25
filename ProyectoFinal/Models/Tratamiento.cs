using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Tratamiento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del tratamiento es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        [MaxLength(500)]
        public string Descripcion { get; set; }
    }
}