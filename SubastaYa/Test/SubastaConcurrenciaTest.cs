using API.Services;
using Application.DTOs.Puja;
using Application.Interfaces;
using Application.UseCases.Pujas;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test
{
    public class SubastaConcurrenciaTest
    {
        [Fact]

            public async Task DosUsuarios_PujanSimultaneamente_DebeManejarConcurrenciaCorrectamente()

            {
            // 1. Configuramos la conexión SQLite en memoria compartida
            var connection = new SqliteConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<SubastaContext>()
                .UseSqlite(connection)
                .Options;

            // Creamos la base de datos e insertamos la subasta inicial (Version = 1)
            using (var setupContext = new SubastaContext(options))
            {
                await setupContext.Database.EnsureCreatedAsync();
                var subastaInicial = new Subasta(
                    vendedorId: 1, categoriaId: 1, titulo: "Test", descripcion: "Desc",
                    urlImagen: null, precioInicial: 100, incrementoMinimo: 10,
                    fechaInicio: DateTime.UtcNow.AddDays(-1), fechaFin: DateTime.UtcNow.AddDays(1)
                );
                setupContext.Subastas.Add(subastaInicial);
                await setupContext.SaveChangesAsync();
            }

            // 2. Simulamos dos usuarios cargando la misma subasta al mismo tiempo (crean contextos separados)
            using var contextUsuarioA = new SubastaContext(options);
            using var contextUsuarioB = new SubastaContext(options);

            var subastaParaA = await contextUsuarioA.Subastas.FirstAsync(s => s.Id == 1);
            var subastaParaB = await contextUsuarioB.Subastas.FirstAsync(s => s.Id == 1);

            // Ambas tienen la misma versión inicial (ej. Version = 1)

            // 3. El Usuario A modifica y guarda primero
            subastaParaA.ExtenderTiempo(DateTime.UtcNow.AddDays(2)); // Esto incrementa Version a 2 internamente
            await contextUsuarioA.SaveChangesAsync(); // Éxito: Se guarda y la BD actualiza a Version = 2

            // 4. El Usuario B intenta guardar su cambio basándose en su versión vieja (Version = 1)
            subastaParaB.ExtenderTiempo(DateTime.UtcNow.AddDays(3));

            // 5. Assert: Debe lanzar la excepción de concurrencia de EF Core
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await contextUsuarioB.SaveChangesAsync();
            });

            await connection.CloseAsync();
        }
        }
    }

