using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configurations
{
    public class ConfiguracionCategoria : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {

            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.Subastas)
                   .WithOne(s => s.Categoria)
                   .HasForeignKey(s => s.CategoriaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
