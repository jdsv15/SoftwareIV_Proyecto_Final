using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class PadecimientoRepository : Repositorio<Padecimiento>, IPadecimientoRepository
    {
        private readonly ApplicationDbContext _db;

        public PadecimientoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Padecimiento padecimiento)
        {
            _db.Padecimientos.Update(padecimiento);
        }
    }
}