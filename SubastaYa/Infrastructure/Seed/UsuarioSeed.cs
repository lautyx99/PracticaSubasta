using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class UsuarioSeed 
    {
       public static void Seed(SubastaContext context)
        {
            if (context.Usuarios.Any())
            {
                return; // Los usuarios ya han sido sembrados
            }

            var fecha = DateTime.Now;

            var usuarios = new List<Usuario>
            {
                new Usuario("vendedor@test.com","Vendedor", "123456", fecha, "Vendedor"),

                new Usuario("comprador1@test.com","Comprador Uno", "123456", fecha, "Comprador"),

                new Usuario("comprador2@test.com","Comprador Dos", "123456", fecha, "Comprador"),

                new Usuario("sinfondos@test.com","Sinfondos", "123456", fecha, "Sinfondos")
            };
            context.Usuarios.AddRange(usuarios);
            context.SaveChanges();
        }
    }
}
