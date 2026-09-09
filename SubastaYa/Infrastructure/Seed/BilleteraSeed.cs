using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class BilleteraSeed
    {
        public static void Seed(SubastaContext context)
        {
            if (context.Billeteras.Any())
                return;

            // Ids según el orden en que se insertaron los usuarios
            var vendedor = context.Usuarios.First(u => u.Email == "vendedor@test.com");
            var comprador1 = context.Usuarios.First(u => u.Email == "comprador1@test.com");
            var comprador2 = context.Usuarios.First(u => u.Email == "comprador2@test.com");
            var sinFondos = context.Usuarios.First(u => u.Email == "sinfondos@test.com");

            var billeteras = new List<Billetera>
        {
            new Billetera(vendedor.Id, 0m, 0m, 1),                          // Saldo $0
            new Billetera(comprador1.Id, 150000m, 45000m, 1),           // Total 150k / Retenido 45k / Disp 105k
            new Billetera(comprador2.Id, 200000m, 0m, 1),               // Total 200k / Disp 200k
            new Billetera(sinFondos.Id, 500m, 0m, 1)                       // Solo $500
        };

            context.Billeteras.AddRange(billeteras);
            context.SaveChanges();
        }
    }
}
