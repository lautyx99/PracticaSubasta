using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Transaccion
{
    public class TransaccionDto
    {
        public int Id { get; set; }

        public int BilleteraId { get; set; }

        public string Tipo { get; set; } = null!;

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public int SubastaId { get; set; }
    }
}
