using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinal.Models
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [MaxLength(20)]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [MaxLength(100)]
        public string Apellidos { get; set; }

        public DateTime? FechaUltimaAtencion { get; set; }

        [MaxLength(20)]
        public string Telefono { get; set; }

        public DateTime FechaNacimiento { get; set; }
    }
}