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

            builder.HasOne(s => s.Vendedor)
                .WithMany()
                .HasForeignKey(s => s.VendedorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Categoria)
                .WithMany()
                .HasForeignKey(s => s.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.PrecioInicial)
                    .HasPrecision(18, 2);

            builder.Property(x => x.IncrementoMinimo)
                   .HasPrecision(18, 2);

        }
    }
}
