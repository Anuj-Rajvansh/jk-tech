using Castle.Core.Configuration;
using DocManagementSystem.Services.Interface.Auth;
using DocManagementSystem.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using DocManagementSystem.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Xunit;
using DocManagementSystem.Shared.Enums;

namespace DocManagementSystem.Test.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTest()
        {
            _authServiceMock = new Mock<IAuthService>();

            var config = new ConfigurationBuilder().Build();
            var tokenRevocationService = new Mock<ITokenRevocationService>().Object;

            _controller = new AuthController(tokenRevocationService, config, null, _authServiceMock.Object);
        }

        [Fact]
        public async Task Register_Returns_ResponseModel()
        {
            var model = new RegisterModel { Username = "userName", Password = "password@123", Role = RolesEnum.User.ToString() };

            _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<RegisterModel>()))
                            .ReturnsAsync(new ResponseModel<RegisterModel> { success = true, Data = model });

            var result = await _controller.Register(model);

            Assert.True(result.success);
            Assert.Equal(model, result.Data);
        }

        [Fact]
        public async Task Login_Returns_ResponseModel()
        {
            var loginModel = new LoginModel { Username = "user", Password = "pass@123" };

            _authServiceMock.Setup(s => s.LoginAsync("user", "pass@123"))
                            .ReturnsAsync(new ResponseModel<LoginModel> { success = true, Data = loginModel });

            var result = await _controller.Login(loginModel);

            Assert.True(result.success);
            Assert.Equal(loginModel, result.Data);
        }
    }
}
