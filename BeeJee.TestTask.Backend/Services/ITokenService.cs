using System.IdentityModel.Tokens.Jwt;

namespace BeeJee.TestTask.Backend.Services
{
    public interface ITokenService
    {
        string GenerateToken(string name);
        JwtSecurityToken GetJwtSecurityToken(string token, bool validateLifetime);
    }
}
