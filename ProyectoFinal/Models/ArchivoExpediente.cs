using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinal.Models
{
    public class ArchivoExpediente
    {
        [Key]
        public int Id { get; set; }

        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        public string NombreArchivo { get; set; }
        public string UrlArchivo { get; set; } 
        public string Descripcion { get; set; }
        public DateTime FechaSubida { get; set; }
        public string MedicoId { get; set; }


    }
}