using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConfiguracionPuja : IEntityTypeConfiguration<Puja>
    {
        public void Configure(EntityTypeBuilder<Puja> builder)
        {
            // Configure the Puja entity

            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Subasta)
                .WithMany()
                .HasForeignKey(p => p.SubastaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Usuario)
                .WithMany()
                .HasForeignKey(u => u.CompradorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Monto)
                .HasPrecision(18, 2);
        
        }
    }
}
