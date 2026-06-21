using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IArchivoExpedienteRepository : IRepositorio<ArchivoExpediente>
    {
        void Actualizar(ArchivoExpediente archivoExpediente);
    }
}