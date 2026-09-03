using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure
{
    public class TransaccionLedger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BilleteraId { get; set; }

        public string Tipo { get; set; }

        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        public int SubastaId { get; set; }

        [ForeignKey("SubastaId")]
        public virtual Subasta Subasta { get; set; }

        [ForeignKey("BilleteraId")]
        virtual public Billetera Billetera { get; set; }

    }
}
