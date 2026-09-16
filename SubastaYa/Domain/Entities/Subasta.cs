using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Subasta
    {
        public int Id { get; private set; }

        public int VendedorId { get; private set; }

        public int CategoriaId { get; private set; }

        public string Titulo { get; private set; } = null!;

        public string Descripcion { get; private set; } = null!;

        public string? UrlImagen { get; private set; }

        public decimal PrecioInicial { get; private set; }

        public decimal IncrementoMinimo { get; private set; }

        public DateTime FechaInicio { get; private set; }

        public DateTime FechaFin { get; private set; }

        public DateTime FechaFinOriginal { get; private set; }

        public bool Finalizada { get; private set; }

        public EstadoSubasta Estado { get; private set; } 

        public int Version { get; private set; }

        public int? GanadorId { get; private set; }
        public decimal? PrecioFinal { get; private set; }

        public virtual Usuario Vendedor { get; private set; } = null!;

        public virtual Categoria Categoria { get; private set; } = null!;

        public virtual ICollection<TransaccionLedger> Transacciones { get; private set; } = new List<TransaccionLedger>();

        public virtual ICollection<Puja> Pujas { get; private set; } = new List<Puja>();


        private Subasta()
        {
            // Required by EF Core
        }
        
        public Subasta(int vendedorId, int categoriaId, string titulo, string descripcion, string? urlImagen, decimal precioInicial, decimal incrementoMinimo, DateTime fechaInicio, DateTime fechaFin)
        {
            VendedorId = vendedorId;
            CategoriaId = categoriaId;
            Titulo = titulo;
            Descripcion = descripcion;
            UrlImagen = urlImagen;
            PrecioInicial = precioInicial;
            IncrementoMinimo = incrementoMinimo;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            FechaFinOriginal = fechaFin;
            Estado = EstadoSubasta.Activa;
            Version = 1;
        }

        // Métodos de dominio

        public void MarcarComoFinalizada(int? ganadorId, decimal? precioFinal)
        {
            Finalizada = true;
            Estado = EstadoSubasta.Finalizada;
            GanadorId = ganadorId;
            PrecioFinal = precioFinal;
        }

        public void MarcarComoDesierta()
        {
            Estado = EstadoSubasta.Desierta;
        }

        public void MarcarComoProxima()
        {
            Estado = EstadoSubasta.Proxima;
        }

        public void ExtenderTiempo(DateTime nuevaFechaFin)
        {
            if (nuevaFechaFin <= FechaFin)
            {
                throw new ArgumentException("La nueva fecha de fin debe ser posterior a la fecha actual de cierre.", nameof(nuevaFechaFin));
            }

            FechaFin = nuevaFechaFin;
            Version++;
        }

        public void MarcarComoCancelada()
        {
            Estado = EstadoSubasta.Cancelada;
        }

        public bool EstaActiva() =>
        Estado == EstadoSubasta.Activa
            && DateTime.UtcNow >= FechaInicio
            && DateTime.UtcNow <= FechaFin;
    }
}
