using Application.DTOs.Puja;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Subasta
{
    public class SubastaDto
    {
        public int Id { get; set; }
        public int VendedorId { get; set; }
        public string? VendedorNombre { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? UrlImagen { get; set; }
        public decimal PrecioInicial { get; set; }
        public decimal IncrementoMinimo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = null!;
        public int? GanadorId { get; set; }          
        public decimal? PrecioFinal { get; set; }     

        public int Version { get; set; }

        public int CantidadPujas { get; set; }

        public PujaDto? MejorPuja { get; set; }    

        public List<PujaDto> Pujas { get; set; } = new();
    }
}
