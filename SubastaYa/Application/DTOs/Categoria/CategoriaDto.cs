using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Categoria
{
    public class CategoriaDto 
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string UrlIcono { get; set; } = null!;
    }
}
