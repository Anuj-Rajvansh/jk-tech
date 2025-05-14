using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Interface.Auth
{
    public interface ITokenRevocationService
    {
        void Revoke(string token);
        bool IsRevoked(string token);
    }
}
