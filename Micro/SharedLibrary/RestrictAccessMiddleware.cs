// using Microsoft.AspNetCore.Http;
// using System.Threading.Tasks;

// namespace SharedLibrary;

// public class RestrictAccessMiddleware(RequestDelegate next )
// {
//     public async Task InvokeAsync(HttpContext context)
//     {
//         var referrer=context.Request.Headers["Referrer"].FirstOrDefault();
//         if(string.IsNullOrEmpty(referrer))
//         {
//             context.Response.StatusCode = StatusCodes.Status403Forbidden;
//             await context.Response.WriteAsync("Hmmm, Cant allow you to  this page");
//             return;
//         }
//         else
//         {
//             await next(context);
//         }
//     }
// }
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class RestrictAccessMiddleware
    {
        private readonly RequestDelegate _next;

        public RestrictAccessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var referrer = context.Request.Headers["Referrer"].FirstOrDefault();
            if (string.IsNullOrEmpty(referrer))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Hmmm, Can't allow you to access this page");
                return;
            }

            await _next(context);
        }
    }
}
