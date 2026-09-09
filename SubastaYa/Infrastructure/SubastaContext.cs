using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class SubastaContext : DbContext
    {

        public SubastaContext(DbContextOptions<SubastaContext> options) : base(options)
        {
        }

        public DbSet<Subasta> Subastas => Set<Subasta>();
        public DbSet<Billetera> Billeteras => Set<Billetera>();
        public DbSet<TransaccionLedger> Transacciones => Set<TransaccionLedger>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Puja> Pujas => Set<Puja>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<AuditoriaLog> AuditoriaLogs => Set<AuditoriaLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubastaContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subasta>()
                .Property(s => s.Estado)
                .HasConversion<string>(); // Configura la conversión de enum a string
        }

    }
}
