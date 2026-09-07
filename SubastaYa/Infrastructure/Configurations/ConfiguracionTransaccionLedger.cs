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

            builder.HasOne(b => b.Billetera)
                .WithMany()
                .HasForeignKey(b => b.BilleteraId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Subasta)
                .WithMany()
                .HasForeignKey(s => s.SubastaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Monto)
                .HasPrecision(18, 2);
        }
    }
}
