using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HotelMe.Shared.Helpers
{
    public static class JwtHelper
    {
        public static string GetUserRole(string token)
        {
            if (string.IsNullOrEmpty(token)) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            Console.WriteLine("Debugging JWT Claims:");
            foreach (var claim in jwt.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            // 🔥 Вземаме всички ролеви claims и вземаме първия
            var roleClaims = jwt.Claims
                .Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .Select(c => c.Value)
                .ToList();

            if (roleClaims.Any())
            {
                Console.WriteLine($"Еxtracted Role Claims: {string.Join(", ", roleClaims)}");
                return roleClaims.First(); // Връщаме първата роля
            }
            else
            {
                Console.WriteLine("Role Claim is MISSING!");
                return null;
            }
        }

    }
}
