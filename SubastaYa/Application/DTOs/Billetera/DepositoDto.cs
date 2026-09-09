using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Billetera
{
    public class DepositoDto
    {
        public int UsuarioId { get; set; }

        public decimal Monto { get; set; }
    }
}
