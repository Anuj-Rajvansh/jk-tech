using DocManagementSystem.Reposetories.Interface.Auth;
using DocManagementSystem.Services.Interface.Auth;
using DocManagementSystem.Shared.DTOs;
using DocManagementSystem.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Implementation.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthRepository authRepository, IConfiguration config, ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _config = config;
            _logger = logger;
        }

        public async Task<ResponseModel<RegisterModel>> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                _logger.LogInformation($"Attempting to register user: {registerModel.Username}");

                var userExist = await _authRepository.GetUser(registerModel.Username);
                if (userExist != null)
                {
                    _logger.LogWarning($"Registration failed - User already exists: {registerModel.Username}");
                    return GenerateResponse("User Exist.", false, registerModel);
                }

                var user = new User
                {
                    UserName = registerModel.Username,
                    PasswordHash = registerModel.Password
                };

                await _authRepository.RegisterAsync(registerModel.Username, registerModel.Password, registerModel.Role);

                var token = GenerateToken(new LoginModel(user));
                _logger.LogInformation($"User registered successfully: {registerModel.Username}");
                return GenerateResponse($"Register Successfully.", true, registerModel, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred during registration for user: {registerModel.Username}");
                throw;
            }
        }

        public async Task<ResponseModel<LoginModel>> LoginAsync(string username, string password)
        {
            try
            {
                _logger.LogInformation($"Login attempt for user: {username}");

                var user = await _authRepository.GetUser(username);
                if (user != null)
                {
                    var token = GenerateToken(user);
                    _logger.LogInformation($"Login successful for user: {username}");
                    return GenerateResponse($"Login Successfully.", true, user, token);
                }
                else
                {
                    _logger.LogWarning($"Login failed - user not found or invalid credentials: {username}");
                    return GenerateResponse<LoginModel>($"Login failed.", false, user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Login error for user: {username}");
                throw;
            }
        }

        private string GenerateToken(LoginModel user)
        {
            _logger.LogDebug($"Generating JWT token for user: {user.Username}");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogDebug($"JWT token generated successfully for user: {user.Username}");

            return tokenString;
        }

        private ResponseModel<T> GenerateResponse<T>(string Message, bool success, T user, string token = null)
        {
            return new ResponseModel<T>
            {
                Token = token,
                Message = Message,
                Data = user,
                success = success,
            };
        }
    }
}
