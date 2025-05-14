using DocManagementSystem.Shared.DTOs;
using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Reposetories
    .Interface.Auth
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(string username, string password, string role);
        Task<string> LoginAsync(string username, string password);

        Task<LoginModel> GetUser(string username);
    }
}
