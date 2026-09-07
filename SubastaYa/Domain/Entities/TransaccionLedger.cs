using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class TransaccionLedger
    {
        public int Id { get; private set; }

        public int BilleteraId { get; private set; }

        public string Tipo { get; private set; } = null!;

        public decimal Monto { get; private set; }

        public DateTime Fecha { get; private set; }

        public int SubastaId { get; private set; } 

        public virtual Subasta Subasta { get; private set; } = null!;
        public virtual Billetera Billetera { get; private set; } = null!;

        private TransaccionLedger() { }

        public TransaccionLedger(int billeteraId, string tipo, decimal monto, DateTime fecha, int subastaId)
        {
            BilleteraId = billeteraId;
            Tipo = tipo;
            Monto = monto;
            Fecha = fecha;
            SubastaId = subastaId;
        }

    }
}
