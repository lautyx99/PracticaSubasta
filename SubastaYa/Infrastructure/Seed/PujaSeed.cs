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
            // 1. Obtener los usuarios de prueba de forma segura
            var comprador1 = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "comprador1@test.com")
                          ?? context.Usuarios.OrderBy(u => u.Id).FirstOrDefault();

            var comprador2 = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "comprador2@test.com")
                          ?? context.Usuarios.Skip(1).OrderBy(u => u.Id).FirstOrDefault()
                          ?? comprador1;

            // 2. Obtener las subastas requeridas de forma segura
            var subastaEstandar = context.Subastas.FirstOrDefault(s => s.Titulo == "iPhone 15 Pro")
                               ?? context.Subastas.OrderBy(s => s.Id).FirstOrDefault();

            var subastaVencidaGanador = context.Subastas.FirstOrDefault(s => s.Titulo == "Notebook Gamer")
                                     ?? context.Subastas.Skip(1).OrderBy(s => s.Id).FirstOrDefault()
                                     ?? subastaEstandar;

            // Guard Clause unificada para eliminar advertencias CS8602
            if (comprador1 == null || comprador2 == null || subastaEstandar == null || subastaVencidaGanador == null)
            {
                return;
            }

            var ahora = DateTime.UtcNow;

            var pujas = new List<Puja>
        {
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
