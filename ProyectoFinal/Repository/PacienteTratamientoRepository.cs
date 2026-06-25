using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class PacienteTratamientoRepository : Repositorio<PacienteTratamiento>, IPacienteTratamientoRepository
    {
        private readonly ApplicationDbContext _db;

        public PacienteTratamientoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(PacienteTratamiento pacienteTratamiento)
        {
            _db.Update(pacienteTratamiento);
        }
    }
}