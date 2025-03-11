using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace HotelMe.Shared.Helpers
{
    public static class JwtHelper
    {
        public static string GetUserRole(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var roleClaim = jwt.Claims.FirstOrDefault(c =>
                c.Type == "role" || c.Type == ClaimTypes.Role ||
                c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            return roleClaim;
        }
    }
}
