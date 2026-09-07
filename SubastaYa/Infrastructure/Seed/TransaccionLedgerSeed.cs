using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class TransaccionLedgerSeed
    {
        public static void Seed(SubastaContext context)
        {
            if (context.Transacciones.Any())
                return;

            var comprador1 = context.Usuarios.First(u => u.Email == "comprador1@test.com");
            var comprador2 = context.Usuarios.First(u => u.Email == "comprador2@test.com");
            var sinFondos = context.Usuarios.First(u => u.Email == "sinfondos@test.com");

            var billeteraComprador1 = context.Billeteras.First(b => b.UsuarioId == comprador1.Id);
            var billeteraComprador2 = context.Billeteras.First(b => b.UsuarioId == comprador2.Id);
            var billeteraSinFondos = context.Billeteras.First(b => b.UsuarioId == sinFondos.Id);

            var subastaEstandar = context.Subastas.First(s => s.Titulo == "iPhone 15 Pro");

            var ahora = DateTime.UtcNow;

            var transacciones = new List<TransaccionLedger>
        {
            // ===== Comprador 1 (Total 150.000 / Retenido 45.000 / Disp 105.000) =====
            // Depósito inicial
            new TransaccionLedger(
                billeteraComprador1.Id,
                "Deposito",
                150000m,
                ahora.AddDays(-2),
                subastaEstandar.Id),

            // Retención por la puja líder de $45.000
            new TransaccionLedger(
                billeteraComprador1.Id,
                "Retencion",
                45000m,
                ahora.AddMinutes(-10),
                subastaEstandar.Id),

            // ===== Comprador 2 (Total 200.000 / Disp 200.000) =====
            new TransaccionLedger(
                billeteraComprador2.Id,
                "Deposito",
                200000m,
                ahora.AddDays(-3),
                subastaEstandar.Id),

            // ===== Sin Fondos (Total 500) =====
            new TransaccionLedger(
                billeteraSinFondos.Id,
                "Deposito",
                500m,
                ahora.AddDays(-1),
                subastaEstandar.Id)
        };

            context.Transacciones.AddRange(transacciones);
            context.SaveChanges();
        }
    }
}
