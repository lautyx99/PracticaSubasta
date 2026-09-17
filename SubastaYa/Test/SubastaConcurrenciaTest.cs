using API.Services;
using Application.DTOs.Puja;
using Application.Interfaces;
using Application.UseCases.Pujas;
using Domain.Entities;
using Domain.Enums;
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

            // Creamos la base de datos e insertamos los datos previos necesarios (Foreign Keys)
            using (var setupContext = new SubastaContext(options))
            {
                await setupContext.Database.EnsureCreatedAsync();

                // 🎯 1. Sembrar el Usuario vendedor (Id = 1) para satisfacer la Foreign Key
                var usuarioVendedor =
                new Usuario(
                email: "vendedor@test.com",
                nombre: "Vendedor de Prueba",
                contraseñaHash: "dummyhash",
                fechaRegistro: DateTime.UtcNow,
                rol: RolUsuario.Vendedor
                );
                setupContext.Usuarios.Add(usuarioVendedor);

                // 🎯 2. Sembrar la Categoría (Id = 1) para satisfacer la Foreign Key
                var categoria = new Categoria
                (
                   nombre : "General",
                   urlIcono : ""
                );
                setupContext.Categorias.Add(categoria);

                await setupContext.SaveChangesAsync();

                // 3. Ahora sí, creamos e insertamos la subasta inicial (Version = 1)
                var subastaInicial = new Subasta(
                    vendedorId: 1,
                    categoriaId: 1,
                    titulo: "Test",
                    descripcion: "Desc",
                    urlImagen: null,
                    precioInicial: 100,
                    incrementoMinimo: 10,
                    fechaInicio: DateTime.UtcNow.AddDays(-1),
                    fechaFin: DateTime.UtcNow.AddDays(1)
                );
                setupContext.Subastas.Add(subastaInicial);
                await setupContext.SaveChangesAsync();
            }

            // 2. Simulamos dos usuarios cargando la misma subasta al mismo tiempo
            using var contextUsuarioA = new SubastaContext(options);
            using var contextUsuarioB = new SubastaContext(options);

            var subastaParaA = await contextUsuarioA.Subastas.FirstAsync(s => s.Id == 1);
            var subastaParaB = await contextUsuarioB.Subastas.FirstAsync(s => s.Id == 1);

            // 3. El Usuario A modifica y guarda primero
            subastaParaA.ExtenderTiempo(DateTime.UtcNow.AddDays(2));
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

