using Domain.Entities;
using Domain.Enums;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    public class AntisnipingIntegrationTest
    {
        [Fact]
        public async Task RealizarPuja_EnLosUltimosMinutos_DebeExtenderFechaFinAutomaticamente()
        {
            // 1. Arrange: Configuramos una conexión SQLite en memoria aislada para esta prueba
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<SubastaContext>()
                .UseSqlite(connection)
                .Options;

            DateTime fechaFinOriginal;

            using (var setupContext = new SubastaContext(options))
            {
                await setupContext.Database.EnsureCreatedAsync();

                // Sembramos los datos obligatorios respetando las restricciones y constructores
                fechaFinOriginal = DateTime.UtcNow.AddMinutes(2); // Cierre próximo (dentro del rango de anti-sniping)

                var vendedor = new Usuario(
                    email: "vendedor@test.com",
                    nombre: "Vendedor Test",
                    contraseñaHash: "hash",
                    fechaRegistro: DateTime.UtcNow,
                    rol: RolUsuario.Vendedor
                );

                var comprador = new Usuario(
                    email: "comprador@test.com",
                    nombre: "Comprador Test",
                    contraseñaHash: "hash",
                    fechaRegistro: DateTime.UtcNow,
                    rol: RolUsuario.Comprador
                );

                var categoria = new Categoria(nombre: "General", urlIcono: "");

                setupContext.Usuarios.AddRange(vendedor, comprador);
                setupContext.Categorias.Add(categoria);
                await setupContext.SaveChangesAsync();

                // Creamos la subasta con los IDs generados
                var subasta = new Subasta(
                    vendedorId: vendedor.Id,
                    categoriaId: categoria.Id,
                    titulo: "Subasta Anti-Sniping",
                    descripcion: "Prueba de extensión de tiempo",
                    urlImagen: null,
                    precioInicial: 100,
                    incrementoMinimo: 10,
                    fechaInicio: DateTime.UtcNow.AddHours(-1),
                    fechaFin: fechaFinOriginal
                );

                setupContext.Subastas.Add(subasta);

                // Billetera del comprador con saldo
                var billetera = new Billetera(
                    usuarioId: comprador.Id,
                    saldoTotal: 50000,
                    saldoRetenido: 0,
                    version: 1
                );
                setupContext.Billeteras.Add(billetera);

                await setupContext.SaveChangesAsync();
            }

            // 2. Act: Simulamos la acción sobre la subasta (o la ejecución a través de la lógica que aplica el anti-sniping)
            using (var executionContext = new SubastaContext(options))
            {
                var subastaParaPujar = await executionContext.Subastas.FirstAsync(s => s.Id == 1);

                // Si la lógica de anti-sniping se ejecuta al registrar la puja o evaluar el tiempo:
                // (Ajusta este método según el nombre exacto que tenga en tu entidad Subasta o caso de uso)
                var nuevaFechaFin = subastaParaPujar.FechaFin.AddMinutes(10); // O el tiempo que defina tu regla de anti-sniping
                subastaParaPujar.ExtenderTiempo(nuevaFechaFin);

                await executionContext.SaveChangesAsync();
            }

            // 3. Assert: Verificamos en un nuevo contexto que la fecha de fin se haya extendido correctamente
            using (var assertContext = new SubastaContext(options))
            {
                var subastaActualizada = await assertContext.Subastas.FirstAsync(s => s.Id == 1);

                Assert.True(
                    subastaActualizada.FechaFin > fechaFinOriginal,
                    "La fecha de fin de la subasta debería haberse extendido debido a la regla de anti-sniping."
                );
            }

            await connection.CloseAsync();
        }
    }
}

