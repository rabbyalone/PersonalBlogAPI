using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Personal.Blog.Domain.ConfigModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Personal.Blog.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOptions<JwtSettings> _jwtSettings;

        public AuthService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateKey(string userSecret)
        {
            if (!string.IsNullOrWhiteSpace(userSecret) && userSecret == _jwtSettings.Value.UserSecret)
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.SecretKey);

                var claims = new List<Claim>
                {
                        new Claim(ClaimTypes.Name, "Rabby Hasan's Blog")
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(300),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Audience = _jwtSettings.Value.Audience,
                    Issuer = _jwtSettings.Value.Issuer,
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            else
            {
                throw new ArgumentException("Please provide a valid User Secret");
            }
        }
    }
}
