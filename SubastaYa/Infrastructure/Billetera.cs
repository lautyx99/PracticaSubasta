using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure
{
    public class Billetera
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int usuarioId { get; set; }

        public decimal SaldoTotal { get; set; }

        public decimal SaldoRetenido { get; set; }

        public decimal SaldoDisponible { get; set; }

        public int Version { get; set; }

        [ForeignKey("usuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
}
