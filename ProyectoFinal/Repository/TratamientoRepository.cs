using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class TratamientoRepository : Repositorio<Tratamiento>, ITratamientoRepository
    {
        private readonly ApplicationDbContext _db;

        public TratamientoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Tratamiento tratamiento)
        {
            _db.Tratamientos.Update(tratamiento);
        }
    }
}