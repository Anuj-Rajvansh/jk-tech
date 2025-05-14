using DocManagementSystem.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Shared.DTOs
{
    public class LoginModel
    {
        public LoginModel() { }
        public LoginModel(User user)
        {
            Username = user.UserName;
            Password = user.PasswordHash;
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
