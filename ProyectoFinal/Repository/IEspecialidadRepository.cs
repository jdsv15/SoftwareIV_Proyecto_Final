using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IEspecialidadRepository : IRepositorio<Especialidad>
    {
        void Update(Especialidad especialidad);
    }
}