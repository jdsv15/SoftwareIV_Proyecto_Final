using ProyectoFinal.Models;


namespace ProyectoFinal.Repository
{
    public interface IUnidadTrabajo : IDisposable
    {
        IEspecialidadRepository Especialidad { get; }

        // Modulo de Medicina
        IPadecimientoRepository Padecimiento { get; }
        ITratamientoRepository Tratamiento { get; }
        IMedicamentoRepository Medicamento { get; }
        IPacienteRepository Paciente { get; }

        //Expediente
        IHistorialClinicoRepository HistorialClinico { get; }
        IArchivoExpedienteRepository ArchivoExpediente { get; }
        IPacientePadecimientoRepository PacientePadecimiento { get; }
        IPacienteTratamientoRepository PacienteTratamiento { get; }
        IPacienteMedicamentoRepository PacienteMedicamento { get; }

        Task SaveAsync();

        void Guardar();
    }
}