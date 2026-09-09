using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConfiguracionBilletera : IEntityTypeConfiguration<Billetera>
    {
        public void Configure(EntityTypeBuilder<Billetera> builder)
        {
            // Configure the Billetera entity

            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Usuario)
                .WithOne(u => u.Billetera)
                .HasForeignKey<Billetera>(b => b.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Transacciones)
                .WithOne(t => t.Billetera)
                .HasForeignKey(t => t.BilleteraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.SaldoTotal)
                    .HasPrecision(18, 2);

            builder.Property(x => x.SaldoRetenido)
                   .HasPrecision(18, 2);

            builder.Property(x => x.SaldoDisponible)
                   .HasPrecision(18, 2);
        }
    }
}
