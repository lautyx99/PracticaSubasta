using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class PujaSeed
    {
        public static void Seed(SubastaContext context)
        {
            if (context.Pujas.Any())
                return;

            var comprador1 = context.Usuarios.First(u => u.Email == "comprador1@test.com");
            var comprador2 = context.Usuarios.First(u => u.Email == "comprador2@test.com");

            var subastaEstandar = context.Subastas.First(s => s.Titulo == "iPhone 15 Pro");
            var subastaVencidaGanador = context.Subastas.First(s => s.Titulo == "Notebook Gamer");

            var ahora = DateTime.UtcNow;

            var pujas = new List<Puja>
        {
            // Subasta activa estándar (líder $45.000)
            new Puja(subastaEstandar.Id, comprador2.Id, 40000m, ahora.AddMinutes(-20)),
            new Puja(subastaEstandar.Id, comprador1.Id, 45000m, ahora.AddMinutes(-10)),

            // Subasta vencida con ganador
            new Puja(subastaVencidaGanador.Id, comprador1.Id, 52000m, ahora.AddMinutes(-30))
        };

            context.Pujas.AddRange(pujas);
            context.SaveChanges();
        }
    }
}
