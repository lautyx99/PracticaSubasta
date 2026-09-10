using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Auth
{
      public record AuthResponseDto(int Id, string Nombre, string Email, string Token);

}
