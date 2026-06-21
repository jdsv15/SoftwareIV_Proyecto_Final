using System.Collections.Generic;

namespace ProyectoFinal.Models.ViewModels
{
    public class ExpedienteVM
    {
        public Paciente? Paciente { get; set; }

        public IEnumerable<HistorialClinico> Historial { get; set; } = new List<HistorialClinico>();

        public IEnumerable<ArchivoExpediente> Archivos { get; set; } = new List<ArchivoExpediente>();

        public IEnumerable<PacientePadecimiento> Padecimientos { get; set; } = new List<PacientePadecimiento>();

        public IEnumerable<PacienteTratamiento> Tratamientos { get; set; } = new List<PacienteTratamiento>();

        public IEnumerable<PacienteMedicamento> Medicamentos { get; set; } = new List<PacienteMedicamento>();
    }
}