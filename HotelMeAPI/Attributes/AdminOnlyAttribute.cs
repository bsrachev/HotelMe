using HotelMe.Shared.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace HotelMeAPI.Attributes
{
    public class AdminOnlyAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            Console.WriteLine($"AUTH HEADER: {authHeader}");

            //if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var userRole = JwtHelper.GetUserRole(token);

            if (userRole != "Admin")
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
