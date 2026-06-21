using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IPacienteMedicamentoRepository : IRepositorio<PacienteMedicamento>
    {
        void Actualizar(PacienteMedicamento pacienteMedicamento);
    }
}