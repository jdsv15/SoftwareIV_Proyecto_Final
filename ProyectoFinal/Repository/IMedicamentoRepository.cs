using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IMedicamentoRepository : IRepositorio<Medicamento>
    {
        void Actualizar(Medicamento medicamento);
    }
}