using ProyectoFinal.Data;

namespace ProyectoFinal.Repository
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;

        public IEspecialidadRepository Especialidad { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Especialidad = new EspecialidadRepository(_db);
        }

        public void Guardar()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}