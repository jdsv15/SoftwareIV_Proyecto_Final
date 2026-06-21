using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class ArchivoExpedienteRepository : Repositorio<ArchivoExpediente>, IArchivoExpedienteRepository
    {
        private readonly ApplicationDbContext _db;

        public ArchivoExpedienteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ArchivoExpediente archivoExpediente)
        {
            _db.Update(archivoExpediente);
        }
    }
}