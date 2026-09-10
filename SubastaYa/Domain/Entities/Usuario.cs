using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; private set; }

        public string Email { get; private set; } = null!;

        public string Nombre { get; private set; } = null!;

        public string ContraseñaHash { get; private set; } = null!;

        public DateTime FechaRegistro { get; private set; }

        public RolUsuario Rol { get; private set; }  

        public virtual Billetera Billetera { get; private set; } = null!;

        public virtual ICollection<Subasta> Subastas { get; private set; } = new List<Subasta>();

        public virtual ICollection<Puja> Pujas { get; private set; } = new List<Puja>();

        public virtual ICollection<AuditoriaLog> AuditoriaLogs { get; private set; } = new List<AuditoriaLog>();

        private Usuario() { }

        public Usuario(string email, string nombre, string contraseñaHash, DateTime fechaRegistro, RolUsuario rol)
        {
            Email = email;
            Nombre = nombre;
            ContraseñaHash = contraseñaHash;
            FechaRegistro = fechaRegistro;
            Rol = rol;
        }

  
    }
}
