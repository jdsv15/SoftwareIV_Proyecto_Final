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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Medico>()
                .HasIndex(m => m.UserId)
                .IsUnique();

            builder.Entity<Medico>()
                .HasMany(m => m.Especialidades)
                .WithMany(e => e.Medicos)
                .UsingEntity(j => j.ToTable("MedicoEspecialidad"));
        }
    }
}