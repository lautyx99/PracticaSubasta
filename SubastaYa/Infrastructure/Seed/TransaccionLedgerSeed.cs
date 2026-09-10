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

            // 1. Obtener usuarios de prueba de forma segura
            var comprador1 = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "comprador1@test.com")
                          ?? context.Usuarios.OrderBy(u => u.Id).FirstOrDefault();

            var comprador2 = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "comprador2@test.com")
                          ?? context.Usuarios.Skip(1).OrderBy(u => u.Id).FirstOrDefault()
                          ?? comprador1;

            var sinFondos = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "sinfondos@test.com")
                         ?? context.Usuarios.Skip(2).OrderBy(u => u.Id).FirstOrDefault()
                         ?? comprador1;

            // Guard Clause 1: Garantiza que los usuarios no son nulos antes de acceder a sus .Id
            if (comprador1 == null || comprador2 == null || sinFondos == null)
                return;

            // 2. Obtener las billeteras asociadas a dichos usuarios
            var billeteraComprador1 = context.Billeteras.FirstOrDefault(b => b.UsuarioId == comprador1.Id)
                                    ?? context.Billeteras.OrderBy(b => b.Id).FirstOrDefault();

            var billeteraComprador2 = context.Billeteras.FirstOrDefault(b => b.UsuarioId == comprador2.Id)
                                    ?? context.Billeteras.Skip(1).OrderBy(b => b.Id).FirstOrDefault()
                                    ?? billeteraComprador1;

            var billeteraSinFondos = context.Billeteras.FirstOrDefault(b => b.UsuarioId == sinFondos.Id)
                                   ?? context.Billeteras.Skip(2).OrderBy(b => b.Id).FirstOrDefault()
                                   ?? billeteraComprador1;

            // 3. Obtener la subasta requerida
            var subastaEstandar = context.Subastas.FirstOrDefault(s => s.Titulo == "iPhone 15 Pro")
                               ?? context.Subastas.FirstOrDefault();

            // Guard Clause 2: Garantiza que las billeteras y subasta existen
            if (billeteraComprador1 == null || billeteraComprador2 == null || billeteraSinFondos == null || subastaEstandar == null)
            {
                return;
            }

            var ahora = DateTime.UtcNow;

            var transacciones = new List<TransaccionLedger>
            {
                // ===== Comprador 1 (Total 150.000 / Retenido 45.000 / Disp 105.000) =====
                new TransaccionLedger(
                    billeteraComprador1.Id,
                    "Deposito",
                    150000m,
                    ahora.AddDays(-2),
                    subastaEstandar.Id),

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
