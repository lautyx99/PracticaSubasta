using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public class SubastaContextFactory : IDesignTimeDbContextFactory<SubastaContext>
    {
        public SubastaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SubastaContext>();

            optionsBuilder.UseSqlServer(
                "Server=Lautaro\\SQLEXPRESS;Database=Subasta;Trusted_Connection=True;TrustServerCertificate=True");

            return new SubastaContext(optionsBuilder.Options);
        }
    }
}
