using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HemTentan.Areas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly IConfiguration configuration;

        // Vi kontrolerar om använder namn finns i vårt system
        public TokenController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        // POST /api/token
        [HttpPost]
        public async Task<ActionResult> CreateAsync(Credentials credentials)
        {
            var user = await userManager.FindByNameAsync(credentials.UserName);

            var hasAccess = await userManager.CheckPasswordAsync(user, credentials.Password);

            if (!hasAccess)
            {
                return Unauthorized(); // 401 Unauthorized
            }

            var token = GenerateToken(user);

            return Ok(new { token });           

        }

        private object GenerateToken(IdentityUser user)
        {
            var signingKey = Convert.FromBase64String(configuration["Token:SigningKey"]);

            var expirationInMinutes = int.Parse(configuration["Token:ExpirationInMinutes"]);

            var claims = new ClaimsIdentity(new List<Claim>
            {
                 new Claim("userid", user.Id.ToString())
            });

            // TODO: Check if user has role "Administrator"
            var isAdministrator = userManager.IsInRoleAsync(user, "Administrator").Result;

            if (isAdministrator)
            {
                claims.AddClaim(new Claim("admin", "true"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
                        
            {
                // (ist) Issued At Time
                IssuedAt = DateTime.UtcNow,

                // (exp) Expiration Time
                Expires = DateTime.UtcNow.AddMinutes(expirationInMinutes),

                Subject = claims,

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(signingKey),
                    SecurityAlgorithms.HmacSha256Signature)

                };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            var token = jwtTokenHandler.WriteToken(jwtSecurityToken);

            return token;
        }
    }
    public class Credentials
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
    }
}
