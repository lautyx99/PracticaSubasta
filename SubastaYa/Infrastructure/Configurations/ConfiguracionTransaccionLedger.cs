using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConfiguracionTransaccionLedger : IEntityTypeConfiguration<TransaccionLedger>
    {
        public void Configure(EntityTypeBuilder<TransaccionLedger> builder)
        {
            // Configure the TransaccionLedger entity

            builder.HasKey(t => t.Id);

            // 1. Relación con Billetera (soluciona BilleteraId1)
            builder.HasOne(t => t.Billetera)
                   .WithMany(b => b.Transacciones) // o .WithMany(b => b.Transacciones) si Billetera tiene la lista
                   .HasForeignKey(t => t.BilleteraId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 2. Relación con Subasta (soluciona SubastaId1)
            builder.HasOne(t => t.Subasta)
                   .WithMany()
                   .HasForeignKey(t => t.SubastaId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 3. Tipos decimales (elimina el warning 30000)
            builder.Property(t => t.Monto)
                   .HasPrecision(18, 2);
        }
    }
}
