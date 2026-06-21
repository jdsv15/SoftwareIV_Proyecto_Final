using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class PacienteRepository : Repositorio<Paciente>, IPacienteRepository
    {
        private readonly ApplicationDbContext _db;

        public PacienteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Paciente paciente)
        {
            _db.Update(paciente);
        }
    }
}