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
            // Evitar duplicación si ya existen transacciones
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

            if (comprador1 == null || comprador2 == null || sinFondos == null)
                return;

            // 2. Obtener las billeteras asociadas (ya persistidas en la BD)
            var billeteraComprador1 = context.Billeteras.FirstOrDefault(b => b.UsuarioId == comprador1.Id)
                                    ?? context.Billeteras.OrderBy(b => b.Id).FirstOrDefault();

            var billeteraComprador2 = context.Billeteras.FirstOrDefault(b => b.UsuarioId == comprador2.Id)
                                    ?? context.Billeteras.Skip(1).OrderBy(b => b.Id).FirstOrDefault();

            var billeteraSinFondos = context.Billeteras.FirstOrDefault(b => b.UsuarioId == sinFondos.Id)
                                   ?? context.Billeteras.Skip(2).OrderBy(b => b.Id).FirstOrDefault();

            // 3. Obtener la subasta requerida
            var subastaEstandar = context.Subastas.FirstOrDefault(s => s.Titulo == "iPhone 15 Pro")
                               ?? context.Subastas.FirstOrDefault();

            // Guard Clause: Validar que todos los elementos existen
            if (billeteraComprador1 == null || billeteraComprador2 == null || billeteraSinFondos == null || subastaEstandar == null)
            {
                return;
            }

            var ahora = DateTime.UtcNow;

            // 4. Crear las transacciones usando los IDs ya persistidos
            var transacciones = new List<TransaccionLedger>
        {
            // ===== Comprador 1 =====
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

            // ===== Comprador 2 =====
            new TransaccionLedger(
                billeteraComprador2.Id,
                "Deposito",
                200000m,
                ahora.AddDays(-3),
                subastaEstandar.Id),

            // ===== Sin Fondos =====
            new TransaccionLedger(
                billeteraSinFondos.Id,
                "Deposito",
                500m,
                ahora.AddDays(-1),
                subastaEstandar.Id)
        };

            // 5. Guardar transacciones únicamente
            context.Transacciones.AddRange(transacciones);
            context.SaveChanges();
        }
    }
}
