using CRN.Application.DTOs.Auth;
using CRN.Application.Interfaces;
using CRN.Application.Settings;
using CRN.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CRN.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IUnitOfWork unitOfWork,
            IOptions<JwtSettings> jwtSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponse> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellationToken = default)
        {
            var existingUsername =
                await _unitOfWork.Users.GetByUsernameAsync(
                    request.Username,
                    cancellationToken);

            if (existingUsername != null)
            {
                throw new InvalidOperationException(
                    "Username already exists.");
            }

            var existingEmail =
                await _unitOfWork.Users.GetByEmailAsync(
                    request.Email,
                    cancellationToken);

            if (existingEmail != null)
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword( request.Password),
                Role = "User",
                CreatedOn = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(
                user,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return await CreateAuthResponseAsync(
                user,
                cancellationToken);
        }

        public async Task<AuthResponse?> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var user =
                await _unitOfWork.Users.GetByUsernameAsync(
                    request.Username,
                    cancellationToken);

            if (user == null)
            {
                return null;
            }

            var passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                return null;
            }

            return await CreateAuthResponseAsync(
                user,
                cancellationToken);
        }

        public async Task<AuthResponse?> RefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            var existingToken =
                await _unitOfWork.RefreshTokens.GetByTokenAsync(
                    refreshToken,
                    cancellationToken);

            if (existingToken == null)
            {
                return null;
            }

            if (existingToken.RevokedOn != null)
            {
                return null;
            }

            if (existingToken.ExpiresOn <= DateTime.UtcNow)
            {
                return null;
            }

            // Revoke old refresh token
            existingToken.RevokedOn = DateTime.UtcNow;

            var response = await CreateAuthResponseAsync(
                existingToken.User,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return response;
        }

        private async Task<AuthResponse> CreateAuthResponseAsync(
            User user,
            CancellationToken cancellationToken)
        {
            var accessTokenExpiresAt =
                DateTime.UtcNow.AddMinutes(
                    _jwtSettings.AccessTokenExpirationMinutes);

            var accessToken = GenerateAccessToken(
                user,
                accessTokenExpiresAt);

            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpirationDays)
            };

            await _unitOfWork.RefreshTokens.AddAsync(
                refreshTokenEntity,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessTokenExpiresAt
            };
        }

        private string GenerateAccessToken(
            User user,
            DateTime expiresAt)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.UniqueName,
                    user.Username),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
