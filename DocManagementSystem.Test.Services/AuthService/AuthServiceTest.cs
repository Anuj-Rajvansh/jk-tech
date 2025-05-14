using DocManagementSystem.Reposetories.Interface.Auth;
using DocManagementSystem.Services.Implementation.Auth;
using DocManagementSystem.Shared.DTOs;
using DocManagementSystem.Shared.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocManagementSystem.Test.Services
{
    public class AuthServiceTest
    {
        private readonly Mock<IAuthRepository> _authRepositoryMock;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private readonly Mock<ILogger<AuthService>> _authServiceLogger;

        public AuthServiceTest()
        {
            _authRepositoryMock = new Mock<IAuthRepository>();

            var configValues = new Dictionary<string, string>
        {
            { "Jwt:Secret", "YourSuperSecretKeyForTestingPurposes123!" }
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            _authService = new AuthService(_authRepositoryMock.Object, _configuration, _authServiceLogger.Object);
        }

        [Fact]
        public async Task RegisterAsync_Returns_Failure_If_User_Exists()
        {
            _authRepositoryMock.Setup(x => x.GetUser(It.IsAny<string>()))
                               .ReturnsAsync(new LoginModel { Username = "existingUser" });

            var model = new RegisterModel { Username = "existingUser", Password = "pass@1234", Role = RolesEnum.User.ToString() };

            var result = await _authService.RegisterAsync(model);

            Assert.False(result.success);
            Assert.Equal("User Exist.", result.Message);
        }

        [Fact]
        public async Task LoginAsync_Returns_Token_If_User_Found()
        {
            _authRepositoryMock.Setup(x => x.GetUser("user1"))
                               .ReturnsAsync(new LoginModel { Username = "user1" });

            var result = await _authService.LoginAsync("user1", "password");

            Assert.True(result.success);
            Assert.NotNull(result.Token);
        }
    }
}
