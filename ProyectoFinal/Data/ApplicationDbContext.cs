using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

namespace ProyectoFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Medico> Medicos { get; set; }

        
        
        //Modulo Medicina
        public DbSet<Padecimiento> Padecimientos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Paciente> Paciente { get; set; }


        public DbSet<HistorialClinico> HistorialClinico { get; set; }
        public DbSet<ArchivoExpediente> ArchivoExpediente { get; set; }
        public DbSet<PacientePadecimiento> PacientePadecimiento { get; set; }
        public DbSet<PacienteTratamiento> PacienteTratamiento { get; set; }
        public DbSet<PacienteMedicamento> PacienteMedicamento { get; set; }
    }
}