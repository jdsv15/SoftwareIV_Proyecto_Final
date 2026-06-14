namespace ProyectoFinal.Repository
{
    public interface IUnidadTrabajo : IDisposable
    {
        IEspecialidadRepository Especialidad { get; }

        void Guardar();
    }
}