using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConfiguracionSubasta : IEntityTypeConfiguration<Subasta>
    {
        public void Configure(EntityTypeBuilder<Subasta> builder)
        {
            // Configure the Subasta entity

            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.Pujas)
                    .WithOne(p => p.Subasta)
                    .HasForeignKey(p => p.SubastaId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Transacciones)
                    .WithOne(t => t.Subasta)
                    .HasForeignKey(t => t.SubastaId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.PrecioInicial)
                    .HasPrecision(18, 2);

            builder.Property(x => x.IncrementoMinimo)
                   .HasPrecision(18, 2);

            builder.Property(s => s.PrecioFinal)
            .HasPrecision(18, 2);

            builder.Property(s => s.Version)
                    .IsConcurrencyToken(); // Habilita el Optimistic Locking en EF Core

        }
    }
}
