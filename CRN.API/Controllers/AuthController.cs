using Asp.Versioning;
using CRN.Application.DTOs.Auth;
using CRN.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRN.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _authService.RegisterAsync(
                    request,
                    cancellationToken);

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await _authService.LoginAsync(
                request,
                cancellationToken);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid username or password."
                });
            }

            return Ok(result);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenRequest request,
            CancellationToken cancellationToken = default)
        {
            var result = await _authService.RefreshTokenAsync(
                request.RefreshToken,
                cancellationToken);

            if (result == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token."
                });
            }

            return Ok(result);
        }
    }
}
