using Application.DTOs.Auth;
using Application.DTOs.Login;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

        Task<AuthResponseDto> RegistrarAsync(RegistroRequestDto request);
    }
}
