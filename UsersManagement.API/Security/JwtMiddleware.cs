using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UsersManagement.API.Models;
using UsersManagement.Domain.Home;

namespace UsersManagement.API.Security
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _appSettings.Issuer,
                    //Here we are creating and using JWT within the same application. In this case base URL is fine - JRozario
                    //If the JWT is created using a web service then this could be the consumer URL - JRozario
                    ValidAudience = _appSettings.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "USERID").Value;

                var user = UserList.SingleOrDefault(x => x.USERID == userId);
                // attach user to context on successful jwt validation
                context.Items["User"] = user;//userService.GetById(userId);
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        List<User> UserList = new List<User>
        {
            new User { USERID = "jsmith@email.com", PASSWORD = "test", EMAILID = "jsmith@email.com", FIRST_NAME = "John", LAST_NAME = "Smith", PHONE = "356-735-2748", ACCESS_LEVEL = Roles.DIRECTOR.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "srob@email.com", PASSWORD = "test", FIRST_NAME = "Steve", LAST_NAME = "Rob", EMAILID = "srob@email.com", PHONE = "567-479-8537", ACCESS_LEVEL = Roles.SUPERVISOR.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "dwill@email.com", PASSWORD = "test", FIRST_NAME = "DJ", LAST_NAME = "Will", EMAILID = "dwill@email.com", PHONE = "599-306-6010", ACCESS_LEVEL = Roles.ANALYST.ToString(), WRITE_ACCESS = "WRITE_ACCESS" },
            new User { USERID = "JBlack@email.com", PASSWORD = "test", FIRST_NAME = "Joe", LAST_NAME = "Black", EMAILID = "JBlack@email.com", PHONE = "764-460-8610", ACCESS_LEVEL = Roles.ANALYST.ToString(), WRITE_ACCESS = "" }
        };
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
