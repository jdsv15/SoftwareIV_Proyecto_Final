using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IMedicoRepositorio : IRepositorio<Medico>
    {
        void Actualizar(Medico medico);
    }
}