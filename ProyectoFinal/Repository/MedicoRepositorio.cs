using ProyectoFinal.Data;
using ProyectoFinal.Models;

namespace ProyectoFinal.Repository
{
    public class MedicoRepositorio : Repositorio<Medico>, IMedicoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public MedicoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Medico medico)
        {
            var medicoBD = _db.Medicos.FirstOrDefault(m => m.Id == medico.Id);
            if (medicoBD != null)
            {
                medicoBD.NumeroColegiado = medico.NumeroColegiado;
                medicoBD.UserId = medico.UserId;

                if (medico.Fotografia != null)
                {
                    medicoBD.Fotografia = medico.Fotografia;
                }

            }
        }
    }
}