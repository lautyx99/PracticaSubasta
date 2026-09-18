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

            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Billetera)
                   .WithMany(b => b.Transacciones) 
                   .HasForeignKey(t => t.BilleteraId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Subasta)
                   .WithMany()
                   .HasForeignKey(t => t.SubastaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(t => t.Monto)
                   .HasPrecision(18, 2);
        }
    }
}
