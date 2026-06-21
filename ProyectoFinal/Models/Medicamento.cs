using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Medicamento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del medicamento es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }
    }
}