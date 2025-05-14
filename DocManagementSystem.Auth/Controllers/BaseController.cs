using DocManagementSystem.Services.Interface.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DocManagementSystem.Auth.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ITokenRevocationService _tokenRevocationService;

        protected BaseController(ITokenRevocationService tokenRevocationService)
        {
            _tokenRevocationService = tokenRevocationService;
        }
        protected string? GetAccessToken()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            return string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ")
                ? null
                : authHeader.Substring("Bearer ".Length).Trim();
        }

        protected ClaimsPrincipal? GetUserClaims()
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var identity = new ClaimsIdentity(jwtToken.Claims);
            return new ClaimsPrincipal(identity);
        }

        protected string? GetUserId()
        {
            var claims = GetUserClaims();
            return claims?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        protected string? GetUserRole()
        {
            var claims = GetUserClaims();
            return claims?.FindFirst(ClaimTypes.Role)?.Value;
        }
        protected string GetTokenFromRequest()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            return authHeader?.StartsWith("Bearer ") == true
                ? authHeader.Substring("Bearer ".Length)
                : null;
        }

        protected void RevokeCurrentToken()
        {
            var token = GetTokenFromRequest();
            if (token != null)
                _tokenRevocationService.Revoke(token);
        }

        protected bool IsCurrentTokenRevoked()
        {
            var token = GetTokenFromRequest();
            return token != null && _tokenRevocationService.IsRevoked(token);
        }
    }
}
