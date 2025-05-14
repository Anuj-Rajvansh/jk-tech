using DocManagementSystem.Services.Interface.Auth;
using DocManagementSystem.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocManagementSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _log;
        private readonly IAuthService _authService;

        public AuthController(ITokenRevocationService tokenRevocationService, IConfiguration config,
                              ILogger<AuthController> log, IAuthService authService)
        : base(tokenRevocationService)
                              
        {
            _config = config;
            _log = log;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ResponseModel<RegisterModel>> Register(RegisterModel model)
        {
            _log.LogInformation($"Register attempt for user: {model.Username}");

            try
            {
                var response = await _authService.RegisterAsync(model);
                _log.LogInformation($"Registration successful for user: {model.Username}");
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Registration failed for user: {model.Username}");
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<ResponseModel<LoginModel>> Login(LoginModel model)
        {
            _log.LogInformation($"Login attempt for user: {model.Username}");

            try
            {
                var response = await _authService.LoginAsync(model.Username, model.Password);
                if (response.success)
                {
                    _log.LogInformation($"Login successful for user: {model.Username}");
                }
                else
                {
                    _log.LogWarning($"Login failed for user: {model.Username} - Reason: {response.Message}");
                }
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Login error for user: {model.Username}");
                throw;
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var username = User?.Identity?.Name ?? "Unknown";
            _log.LogInformation($"Logout initiated for user: {username}");

            try
            {
                RevokeCurrentToken();
                _log.LogInformation($"Logout successful for user: {username}"  );
                return Ok("Logout Successfully.");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, $"Logout failed for user: {username}");
                return StatusCode(500, "Logout failed due to internal error.");
            }
        }
    }
}
