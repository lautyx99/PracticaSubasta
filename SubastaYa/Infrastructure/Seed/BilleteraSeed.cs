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

            // Obtener usuarios de forma segura con fallback por posición
            var vendedor = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "vendedor@test.com")
                        ?? context.Usuarios.OrderBy(u => u.Id).FirstOrDefault();

            var comprador1 = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "comprador1@test.com")
                          ?? context.Usuarios.Skip(1).OrderBy(u => u.Id).FirstOrDefault()
                          ?? vendedor;

            var comprador2 = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "comprador2@test.com")
                          ?? context.Usuarios.Skip(2).OrderBy(u => u.Id).FirstOrDefault()
                          ?? vendedor;

            var sinFondos = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "sinfondos@test.com")
                         ?? context.Usuarios.Skip(3).OrderBy(u => u.Id).FirstOrDefault()
                         ?? vendedor;

            if (vendedor == null || comprador1 == null || comprador2 == null || sinFondos == null) return;

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
