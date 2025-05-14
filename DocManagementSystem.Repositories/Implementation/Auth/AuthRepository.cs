using DocManagementSystem.Reposetories.Interface.Auth;
using DocManagementSystem.Shared.Data;
using DocManagementSystem.Shared.DTOs;
using DocManagementSystem.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mysqlx.Expr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Reposetories.Implementation.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AuthRepository> _logger;
        private readonly IPasswordHasher<LoginModel> _passwordHasher;

        public AuthRepository(ApplicationDbContext dbContext,
                              ILogger<AuthRepository> logger,
                              IPasswordHasher<LoginModel> passwordHasher)
        {
            _dbContext = dbContext;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginModel> GetUser(string username)
        {
            try
            {
                _logger.LogInformation("Fetching user by username: {Username}", username);
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    _logger.LogWarning("User not found: {Username}", username);
                    return null;
                }

                _logger.LogInformation("User found: {Username}", username);
                return new LoginModel(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user: {Username}", username);
                throw;
            }
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            try
            {
                _logger.LogInformation("Login attempt for user: {Username}", username);
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    _logger.LogWarning("Login failed - User not found: {Username}", username);
                    return null;
                }

                var result = _passwordHasher.VerifyHashedPassword(new LoginModel(user), user.PasswordHash, password);
                if (result == PasswordVerificationResult.Success)
                {
                    _logger.LogInformation("Login successful for user: {Username}", username);
                    return user.UserName;
                }

                _logger.LogWarning("Login failed - Invalid password for user: {Username}", username);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error for user: {Username}", username);
                throw;
            }
        }

        public async Task<string> RegisterAsync(string username, string password, string role)
        {
            try
            {
                _logger.LogInformation("Registering new user: {Username}", username);

                var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed - User already exists: {Username}", username);
                    return null;
                }

                User? newUser = new User
                {
                    UserName = username
                };

                newUser.PasswordHash = _passwordHasher.HashPassword(new LoginModel(newUser), password);

                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("User registered successfully: {Username}", username);
                return newUser.UserName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user: {Username}", username);
                throw;
            }
        }
    }
}
