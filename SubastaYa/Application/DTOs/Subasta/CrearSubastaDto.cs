using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Subasta
{
    public class CrearSubastaDto
    {
        public int CategoriaId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string? UrlImagen { get; set; }
        public decimal PrecioInicial { get; set; }
        public decimal IncrementoMinimo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
