namespace ProyectoFinal.Repository
{
    public interface IUnidadTrabajo : IDisposable
    {
        IEspecialidadRepository Especialidad { get; }

        Task SaveAsync();

        void Guardar();
    }
}