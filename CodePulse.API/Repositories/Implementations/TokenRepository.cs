using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodePulse.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)  //Iconfiguration is used to read the appsettings value for JWT Token Creation
        {
            this.configuration = configuration;
        }

        //In this method we create JWTToken
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            // Create Claims for token by using EMail & Role
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Getting key from Appsetting
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            //Signing the Key to make it as unique credential
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Setting the token
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            // Return Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
