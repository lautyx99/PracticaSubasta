using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Puja
    {
        public int Id { get; private set; }

        public int SubastaId { get; private set; }

        public int CompradorId { get; private set; }

        public decimal Monto { get; private set; }

        public DateTime Fecha_Puja { get; private set; }

        public virtual Subasta Subasta { get; private set; }

        public virtual Usuario Usuario { get; private set; }

        private Puja() { }

        public Puja(int subastaId, int compradorId, decimal monto, DateTime fecha_Puja)
        {
            SubastaId = subastaId;
            CompradorId = compradorId;
            Monto = monto;
            Fecha_Puja = fecha_Puja;
        }

    }
}
