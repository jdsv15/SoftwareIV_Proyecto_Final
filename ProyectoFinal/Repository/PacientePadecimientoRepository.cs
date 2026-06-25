using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class PacientePadecimientoRepository : Repositorio<PacientePadecimiento>, IPacientePadecimientoRepository
    {
        private readonly ApplicationDbContext _db;

        public PacientePadecimientoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(PacientePadecimiento pacientePadecimiento)
        {
            _db.Update(pacientePadecimiento);
        }
    }
}