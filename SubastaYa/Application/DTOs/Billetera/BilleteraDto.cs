using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Billetera
{
    public class BilleteraDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal SaldoTotal { get; set; }
        public decimal SaldoRetenido { get; set; }
        public decimal SaldoDisponible { get; set; }
        public int Version { get; set; } // Nueva propiedad para la versión de la billetera
    }
}
