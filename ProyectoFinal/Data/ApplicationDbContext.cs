using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;

namespace ProyectoFinal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Medico> Medicos { get; set; }

        // Módulo Medicina
        public DbSet<Padecimiento> Padecimientos { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Paciente> Paciente { get; set; }

        // Expediente
        public DbSet<HistorialClinico> HistorialClinico { get; set; }
        public DbSet<ArchivoExpediente> ArchivoExpediente { get; set; }
        public DbSet<PacientePadecimiento> PacientePadecimiento { get; set; }
        public DbSet<PacienteTratamiento> PacienteTratamiento { get; set; }
        public DbSet<PacienteMedicamento> PacienteMedicamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Medico)
                .WithMany()
                .HasForeignKey(p => p.MedicoId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}