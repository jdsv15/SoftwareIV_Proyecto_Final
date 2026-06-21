using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface ITratamientoRepository : IRepositorio<Tratamiento>
    {
        void Actualizar(Tratamiento tratamiento);
    }
}
