using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConfiguracionUsuario : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            // Configure the Usuario entity

            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Subastas)
                   .WithOne(s => s.Vendedor)
                   .HasForeignKey(s => s.VendedorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Pujas)
                   .WithOne(p => p.Usuario)
                   .HasForeignKey(p => p.CompradorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.AuditoriaLogs)
                   .WithOne(a => a.Usuario)
                   .HasForeignKey(a => a.UsuarioId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(u => u.Email).IsUnique();

        }
    }
}
