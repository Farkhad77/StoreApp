using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;


namespace StoreApp.Application.Shared.Helpers
{
    public static class JwtHelper
    {
        public static TimeSpan GetTokenExpiry(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var expiry = jwtToken.ValidTo;
            return expiry - DateTime.UtcNow;
        }
    } 
}
