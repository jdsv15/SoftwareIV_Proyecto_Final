using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public interface IPacienteRepository : IRepositorio<Paciente>
    {
        void Actualizar(Paciente paciente);
    }
}