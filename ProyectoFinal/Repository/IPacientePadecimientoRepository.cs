using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IPacientePadecimientoRepository : IRepositorio<PacientePadecimiento>
    {
        void Actualizar(PacientePadecimiento pacientePadecimiento);
    }
}