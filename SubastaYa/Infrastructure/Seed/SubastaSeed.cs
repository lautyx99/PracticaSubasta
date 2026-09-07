using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Seed
{
    public class SubastaSeed
    {
        public static void Seed(SubastaContext context)
        {
            if (context.Subastas.Any())
                return;

            var vendedor = context.Usuarios.First(u => u.Email == "vendedor@test.com");
            var tecnologia = context.Categorias.First(c => c.Nombre == "Tecnología");
            var coleccionables = context.Categorias.First(c => c.Nombre == "Coleccionables");
            var indumentaria = context.Categorias.First(c => c.Nombre == "Indumentaria");
            var vehiculos = context.Categorias.First(c => c.Nombre == "Vehículos");

            var ahora = DateTime.UtcNow;

            // 1. Activa estándar (cierra en 25 min)
            var subastaEstandar = new Subasta(
                vendedor.Id, tecnologia.Id,
                "iPhone 15 Pro", "Subasta activa estándar", urlImagen: null,
                30000m, 1000m,
                ahora.AddMinutes(-30), ahora.AddMinutes(25));

            // 2. Activa crítica (cierra en ~1.5 min)
            var subastaCritica = new Subasta(
                vendedor.Id, coleccionables.Id,
                "Figura Star Wars", "Subasta crítica anti-sniping", urlImagen: null,
                10000m, 500m,
                ahora.AddMinutes(-60), ahora.AddMinutes(1.5));

            // 3. Próxima (inicia en +24 hs)
            var subastaProxima = new Subasta(
                vendedor.Id, indumentaria.Id,
                "Zapatillas Edición Limitada", "Subasta programada", urlImagen: null,
                20000m, 1000m,
                ahora.AddHours(24), ahora.AddHours(48));
            subastaProxima.MarcarComoProxima();

            // 4. Vencida con ganador
            var subastaVencidaGanador = new Subasta(
                vendedor.Id, tecnologia.Id,
                "Notebook Gamer", "Subasta vencida con ganador", urlImagen: null,
                40000m, 2000m,
                ahora.AddHours(-5), ahora.AddMinutes(-10));
            subastaVencidaGanador.MarcarComoFinalizada();

            // 5. Vencida desierta
            var subastaDesierta = new Subasta(
                vendedor.Id, vehiculos.Id,
                "Bicicleta Mountain Bike", "Subasta desierta", urlImagen: null,
                25000m, 1000m,
                ahora.AddHours(-5), ahora.AddMinutes(-10));
            subastaDesierta.MarcarComoDesierta();

            context.Subastas.AddRange(
                subastaEstandar,
                subastaCritica,
                subastaProxima,
                subastaVencidaGanador,
                subastaDesierta);

            context.SaveChanges();
        }
    }
    }
