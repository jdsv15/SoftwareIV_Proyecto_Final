using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IPadecimientoRepository : IRepositorio<Padecimiento>
    {
        void Actualizar(Padecimiento padecimiento);
    }
}