using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class SubastaContext : DbContext
    {
        public SubastaContext(DbContextOptions<SubastaContext> options) : base(options)
        {

        } 

        public DbSet<Subasta> Subastas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Puja> Pujas { get; set; }
        public DbSet<Billetera> Billeteras { get; set; }
        public DbSet<TransaccionLedger> TransaccionesLedger { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }

    }
}
