using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class PacienteMedicamentoRepository : Repositorio<PacienteMedicamento>, IPacienteMedicamentoRepository
    {
        private readonly ApplicationDbContext _db;

        public PacienteMedicamentoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(PacienteMedicamento pacienteMedicamento)
        {
            _db.Update(pacienteMedicamento);
        }
    }
}