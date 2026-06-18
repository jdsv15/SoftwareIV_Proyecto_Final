namespace ProyectoFinal.Repository
{
    public interface IUnidadTrabajo : IDisposable
    {
        IEspecialidadRepository Especialidad { get; }

        IMedicoRepositorio Medico { get; }

        Task SaveAsync();

        void Guardar();
    }
}