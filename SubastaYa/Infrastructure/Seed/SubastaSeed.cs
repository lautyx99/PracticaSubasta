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

            // 1. Obtener los usuarios de prueba requeridos (de forma segura)
            var vendedor = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "vendedor@test.com")
                        ?? context.Usuarios.OrderBy(u => u.Id).FirstOrDefault();

            var comprador = context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == "comprador@test.com")
                         ?? context.Usuarios.Skip(1).OrderBy(u => u.Id).FirstOrDefault()
                         ?? vendedor;


            // 2. Obtener las categorías (usando FirstOrDefault con fallback a la primera categoría disponible)
            var tecnologia = context.Categorias.FirstOrDefault(c => c.Nombre == "Tecnología")
                          ?? context.Categorias.OrderBy(c => c.Id).FirstOrDefault();

            var coleccionables = context.Categorias.FirstOrDefault(c => c.Nombre == "Coleccionables")
                             ?? context.Categorias.OrderBy(c => c.Id).FirstOrDefault();

            var indumentaria = context.Categorias.FirstOrDefault(c => c.Nombre == "Indumentaria")
                            ?? context.Categorias.OrderBy(c => c.Id).FirstOrDefault();

            var vehiculos = context.Categorias.FirstOrDefault(c => c.Nombre == "Vehículos")
                         ?? context.Categorias.OrderBy(c => c.Id).FirstOrDefault();

            if (vendedor == null || comprador == null || tecnologia == null || coleccionables == null || indumentaria == null || vehiculos == null)
            {
                return;
            }

            var ahora = DateTime.UtcNow;

            // 1. Activa estándar (cierra en 25 min)
            var subastaEstandar = new Subasta(
                vendedor.Id, tecnologia.Id,
                "iPhone 15 Pro", "Subasta activa estándar", urlImagen: "https://images.unsplash.com/photo-1736173155811-e8142fd553ee?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                30000m, 1000m,
                ahora.AddMinutes(-30), ahora.AddMinutes(25));

            // 2. Activa crítica (cierra en ~1.5 min)
            var subastaCritica = new Subasta(
                vendedor.Id, coleccionables.Id,
                "Figura Star Wars", "Subasta crítica anti-sniping", urlImagen: "https://images.unsplash.com/photo-1646009759852-f035b994a783?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                10000m, 500m,
                ahora.AddMinutes(-60), ahora.AddMinutes(1.5));

            // 3. Próxima (inicia en +24 hs)
            var subastaProxima = new Subasta(
                vendedor.Id, indumentaria.Id,
                "Zapatillas Edición Limitada", "Subasta programada", urlImagen: "https://images.unsplash.com/photo-1656944227480-98180d2a5155?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                20000m, 1000m,
                ahora.AddHours(24), ahora.AddHours(48));
            subastaProxima.MarcarComoProxima();

            // 4. Vencida con ganador
            var subastaVencidaGanador = new Subasta(
                vendedor.Id, tecnologia.Id,
                "Notebook Gamer", "Subasta vencida con ganador", urlImagen: "https://images.unsplash.com/photo-1771015310937-6754da25e49a?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                40000m, 2000m,
                ahora.AddHours(-5), ahora.AddMinutes(-10));

            // Marcar como finalizada asociando el ID del comprador y el precio final
            subastaVencidaGanador.MarcarComoFinalizada(ganadorId: comprador.Id, precioFinal: 45000m);

            // 5. Vencida desierta (Sin ofertas)
            var subastaDesierta = new Subasta(
                vendedor.Id, vehiculos.Id,
                "Bicicleta Mountain Bike", "Subasta desierta", urlImagen: "https://plus.unsplash.com/premium_photo-1678718713393-2b88cde9605b?q=80&w=1470&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                25000m, 1000m,
                ahora.AddHours(-5), ahora.AddMinutes(-10));

            // Finalizar sin ganador ni precio final
            subastaDesierta.MarcarComoFinalizada(ganadorId: null, precioFinal: null);

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
