using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IPacienteTratamientoRepository : IRepositorio<PacienteTratamiento>
    {
        void Actualizar(PacienteTratamiento pacienteTratamiento);
    }
}