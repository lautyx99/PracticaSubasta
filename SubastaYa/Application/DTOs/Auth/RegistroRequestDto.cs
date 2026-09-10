using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Auth
{
    public class RegistroRequestDto
    {
        public string Nombre { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
