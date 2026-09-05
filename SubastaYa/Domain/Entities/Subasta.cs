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

        public string Estado { get; private set; } = null!;

        public int Version { get; private set; }

        public virtual Usuario Vendedor { get; private set; }

        public virtual Categoria Categoria { get; private set; }

        private Subasta()
        {
            // Required by EF Core
        }
        
        public Subasta(int vendedorId, int categoriaId, string titulo, string descripcion, string urlImagen, decimal precioInicial, decimal incrementoMinimo, DateTime fechaInicio, DateTime fechaFin)
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
            Estado = "Activa";
            Version = 1;
        }

    }
}
