using DocManagementSystem.Services.Interface.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocManagementSystem.Services.Implementation.Auth
{
    public class InMemoryTokenRevocationService : ITokenRevocationService
    {
        private readonly HashSet<string> _revokedTokens = new();

        public void Revoke(string token)
        {
            _revokedTokens.Add(token);
        }

        public bool IsRevoked(string token)
        {
            return _revokedTokens.Contains(token);
        }
    }
}
