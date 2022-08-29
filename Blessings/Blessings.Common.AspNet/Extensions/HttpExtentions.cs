using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blessings.Common.AspNet.Extensions
{
    public static class HttpExtentions
    {
        public static int GetCurrentUserId(this HttpContext context)
        {
            var userIdString = context?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdString, out var userId);
            return userId;
        }
    }
}
