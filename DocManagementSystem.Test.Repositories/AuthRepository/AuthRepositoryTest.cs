using DocManagementSystem.Reposetories.Implementation.Auth;
using DocManagementSystem.Reposetories.Interface;
using DocManagementSystem.Shared.Data;
using DocManagementSystem.Shared.DTOs;
using DocManagementSystem.Shared.Enums;
using DocManagementSystem.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocManagementSystem.Test.Repositories
{
    public class AuthRepositoryTest
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthRepository _repository;
        private readonly Mock<ILogger<AuthRepository>> _mockLogger;
        private readonly Mock<IPasswordHasher<LoginModel>> _mockPasswordHasher;

        public AuthRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Users.Add(new User { UserName = "testuser", PasswordHash = "testpass" });
            _context.SaveChanges();

            _repository = new AuthRepository(_context, _mockLogger.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task GetUser_Returns_User_If_Exists()
        {
            var result = await _repository.GetUser("testuser");
            Assert.NotNull(result);
            Assert.Equal("testuser", result.Username);
        }

        [Fact]
        public async Task RegisterAsync_Adds_User()
        {
            var result = await _repository.RegisterAsync("Ankit", "Ankit@123", RolesEnum.User.ToString());
            Assert.Equal("Ankit", result);
        }
    }
}
