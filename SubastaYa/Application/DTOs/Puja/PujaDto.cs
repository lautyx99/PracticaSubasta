using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Puja
{
    public class PujaDto
    {
        public int Id { get; set; }
        public int SubastaId { get; set; }
        public int CompradorId { get; set; }
        public string? CompradorNombre { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPuja { get; set; }
    }
}
