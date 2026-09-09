using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Subasta
{
    public class CrearSubastaDto
    {
        public int VendedorId { get; init; }
        public int CategoriaId { get; init; }
        public string Titulo { get; init; } = null!;
        public string Descripcion { get; init; } = null!;
        public string? UrlImagen { get; init; }
        public decimal PrecioInicial { get; init; }
        public decimal IncrementoMinimo { get; init; }
        public DateTime FechaInicio { get; init; }
        public DateTime FechaFin { get; init; }
    }
}
