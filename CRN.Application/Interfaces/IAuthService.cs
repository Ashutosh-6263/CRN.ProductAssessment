using CRN.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellationToken = default);

        Task<AuthResponse?> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default);

        Task<AuthResponse?> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);
    }
}
