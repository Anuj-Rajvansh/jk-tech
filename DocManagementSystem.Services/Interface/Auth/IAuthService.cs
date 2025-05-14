using DocManagementSystem.Shared.DTOs;
using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Interface.Auth
{
    public interface IAuthService
    {
        //Task<string?> GetRoleForUserAsync(string userId);

        Task<ResponseModel<RegisterModel>> RegisterAsync(RegisterModel registerModel);
        Task<ResponseModel<LoginModel>> LoginAsync(string username, string password);
    }
}
