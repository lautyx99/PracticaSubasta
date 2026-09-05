using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConfiguracionAuditoriaLog : IEntityTypeConfiguration<AuditoriaLog>
    {
        public void Configure(EntityTypeBuilder<AuditoriaLog> builder)
        {
            // Configure the AuditoriaLog entity}


            builder.HasKey(a => a.Id);

            builder.HasOne(u => u.Usuario)
                .WithMany()
                .HasForeignKey(u => u.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
