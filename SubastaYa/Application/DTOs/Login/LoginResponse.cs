using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Login
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Rol { get; set; } = null!;
    }
}
