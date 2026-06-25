using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class MedicamentoRepository : Repositorio<Medicamento>, IMedicamentoRepository
    {
        private readonly ApplicationDbContext _db;

        public MedicamentoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Medicamento medicamento)
        {
            _db.Medicamentos.Update(medicamento);
        }
    }
}