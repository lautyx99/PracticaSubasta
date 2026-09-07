using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class CategoriaSeed
    {
        public static void Seed(SubastaContext context)
        {
            if (context.Categorias.Any())
                return;

            var categorias = new List<Categoria>
        {
            new Categoria("Tecnología", ""),
            new Categoria("Coleccionables", ""),
            new Categoria("Indumentaria", ""),
            new Categoria("Vehículos", "")
        };

            context.Categorias.AddRange(categorias);
            context.SaveChanges();
        }
    }
}
