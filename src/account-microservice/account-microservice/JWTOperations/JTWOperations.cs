using account_microservice.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace account_microservice.JWTOperations
{
    public class JTWOperations
    {
        private readonly IConfiguration _configuration;
        static UserManager<ApplicationUser> userManager;

        public JTWOperations(IConfiguration configuration, UserManager<ApplicationUser> usermanager)
        {
            _configuration = configuration;
            userManager = usermanager;
        }

        public bool ValidateToken(string token, string Role)
        {
            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var list = jwtToken.Claims.ToList();
                var roleList = new List<string>();
                foreach (var item in list)
                {
                    if (item.Type == ClaimTypes.Role)
                    {
                        roleList.Add(item.Value);
                    }
                }

                // return user id from JWT token if validation successful
                if (roleList.Contains(Role))
                {
                    Console.WriteLine("true");
                    return true;


                }
                Console.WriteLine("false");
                return false;
            }
            catch
            {
                // return false if validation fails
                Console.WriteLine("catch");
                return false;
            }
        }
        public string GenerateTokenAsync(ApplicationUser user)
        {
            var dbuser =  userManager.FindByNameAsync(user.UserName).Result;
            if (dbuser != null)
            {
                var userRoles =  userManager.GetRolesAsync(user).Result;

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null;
        
            
        }
    }
}
