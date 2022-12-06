using BeeJee.TestTask.Backend.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BeeJee.TestTask.Backend.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthOptions _authOptions;
        public TokenService(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions.Value;
        }
        public string GenerateToken(string name)
        {
            var claims = GetIdentity(name)?.Claims ?? throw new ArgumentNullException(nameof(ClaimsIdentity.Claims), "Claims is null");
            return GenerateToken(claims: claims,
                expiresTime: DateTime.UtcNow.Add(TimeSpan.FromHours(_authOptions.AccessTokenLifeTimeHours)));
        }

        public JwtSecurityToken GetJwtSecurityToken(string token, bool validateLifetime)
        {
            var tokenValidationParameters = _authOptions.GetTokenValidationParameters(validateLifetime);
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return tokenHandler.ReadJwtToken(token);
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime expiresTime)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _authOptions.Issuer,
                    audience: _authOptions.Audience,
                    notBefore: now,
                    claims: claims,
                    expires: expiresTime,
                    signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static ClaimsIdentity GetIdentity(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name), "GetIdentity can't bee invoke for null value of name");
            }

            var claims = new List<Claim>
                {
                    new Claim("name", name)
                };

            return new ClaimsIdentity(claims, "Token");
        }
    }
}
