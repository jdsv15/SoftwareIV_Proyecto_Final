using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class HistorialClinicoRepository : Repositorio<HistorialClinico>, IHistorialClinicoRepository
    {
        private readonly ApplicationDbContext _db;

        public HistorialClinicoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(HistorialClinico historialClinico)
        {
            _db.Update(historialClinico);
        }
    }
}