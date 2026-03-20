using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }


        /// <summary>
        /// Generates a signed JWT token for the given user.
        /// Token contains: UserId, UserName, Email, Role as claims.
        /// </summary>
        /// <param name="user"> The authenticated user object from the database. </param>
        /// <returns> A tuple containing the JWT token string and its expiry datetime. </returns>
        /// <exception cref="Exception"> Thrown when Jwt:Key is missing from appsettings.json. </exception>

        public (string token, DateTime expiresAt) GenerateToken(User user)
        {
            // Read settings from appsettings.json --> Jwt section
            var secret = _config["Jwt:Key"] ?? throw new Exception("Jwt:Key not configured");
            var issuer = _config["Jwt:Issuer"] ?? "SchoolManagementSystem";
            var audience = _config["Jwt:Audience"] ?? "SchoolManagementSystem";
            var expireHours = int.Parse(_config["Jwt:ExpireHours"] ?? "24");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims packed into the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiresAt = DateTime.UtcNow.AddHours(expireHours);


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
    }
}
