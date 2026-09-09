using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Puja
{
    public class PujaResultadoDto
    {
        public int Id { get; init; }
        public int SubastaId { get; init; }
        public int CompradorId { get; init; }
        public decimal Monto { get; init; }
        public DateTime FechaUtc { get; init; }
        public bool TiempoExtendido { get; init; }
        public DateTime NuevaFechaFin { get; init; }
    }
}
