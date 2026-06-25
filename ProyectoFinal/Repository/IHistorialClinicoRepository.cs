using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IHistorialClinicoRepository : IRepositorio<HistorialClinico>
    {
        void Actualizar(HistorialClinico historialClinico);
    }
}