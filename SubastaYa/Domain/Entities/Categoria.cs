using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Categoria
    {
        public int Id { get; private set; }

        public string Nombre { get; private set; } = null!;

        public string UrlIcono { get; private set; } = null!;

        public virtual ICollection<Subasta> Subastas { get; private set; } = new List<Subasta>();


        private Categoria() { }

        public Categoria(string nombre, string urlIcono)
        {
            Nombre = nombre;
            UrlIcono = urlIcono;
        }
    }
}
