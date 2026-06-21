using System.Collections.Generic;

namespace ProyectoFinal.Models.ViewModels
{
    public class ExpedienteVM
    {
        public Paciente Paciente { get; set; }
        public IEnumerable<HistorialClinico> Historial { get; set; }
        public IEnumerable<ArchivoExpediente> Archivos { get; set; }
        public IEnumerable<PacientePadecimiento> Padecimientos { get; set; }
        public IEnumerable<PacienteTratamiento> Tratamientos { get; set; }
        public IEnumerable<PacienteMedicamento> Medicamentos { get; set; }
    }
}