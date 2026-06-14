using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class EspecialidadRepository : Repositorio<Especialidad>, IEspecialidadRepository
    {
        private readonly ApplicationDbContext _db;

        public EspecialidadRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Especialidad especialidad)
        {
            var objDesdeDb = _db.Especialidades.FirstOrDefault(s => s.Id == especialidad.Id);

            if (objDesdeDb != null)
            {
                objDesdeDb.Nombre = especialidad.Nombre;
            }
        }
    }
}