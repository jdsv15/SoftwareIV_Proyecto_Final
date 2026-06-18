using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IMedicoRepositorio Medico { get; private set; }

        public IEspecialidadRepository Especialidad { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Especialidad = new EspecialidadRepository(_db);
            Medico = new MedicoRepositorio(_db);
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