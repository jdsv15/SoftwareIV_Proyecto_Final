using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IMedicoRepositorio Medico { get; private set; }
        public IEspecialidadRepository Especialidad { get; private set; }

        //repositorios modulo medicina
        public IPadecimientoRepository Padecimiento { get; private set; }
        public ITratamientoRepository Tratamiento { get; private set; }
        public IMedicamentoRepository Medicamento { get; private set; }
        public IPacienteRepository Paciente { get; private set; }

        //repositorios de expediente
        public IHistorialClinicoRepository HistorialClinico { get; private set; }
        public IArchivoExpedienteRepository ArchivoExpediente { get; private set; }
        public IPacientePadecimientoRepository PacientePadecimiento { get; private set; }
        public IPacienteTratamientoRepository PacienteTratamiento { get; private set; }
        public IPacienteMedicamentoRepository PacienteMedicamento { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Especialidad = new EspecialidadRepository(_db);
            Medico = new MedicoRepositorio(_db); 

            // Inicializacion modulo medicina
            Padecimiento = new PadecimientoRepository(_db);
            Tratamiento = new TratamientoRepository(_db);
            Medicamento = new MedicamentoRepository(_db);
            Paciente = new PacienteRepository(_db);

            //expediente
            HistorialClinico = new HistorialClinicoRepository(_db);
            ArchivoExpediente = new ArchivoExpedienteRepository(_db);
            PacientePadecimiento = new PacientePadecimientoRepository(_db);
            PacienteTratamiento = new PacienteTratamientoRepository(_db);
            PacienteMedicamento = new PacienteMedicamentoRepository(_db);
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