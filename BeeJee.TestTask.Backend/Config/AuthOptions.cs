using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BeeJee.TestTask.Backend.Config
{
    public class AuthOptions
    {
        /// <summary>
        /// Token issuer
        /// </summary>
        public string? Issuer { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string? Audience { get; set; }

        /// <summary>
        /// Encryption key
        /// </summary>
        public string? Key { get; set; }

        /// <summary>
        /// Token life time hours
        /// </summary>
        public int AccessTokenLifeTimeHours { get; set; }

        public TokenValidationParameters GetTokenValidationParameters(bool validateLifetime = true)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = validateLifetime,

                IssuerSigningKey = GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key ?? throw new NullReferenceException("Encryption key value is null")));
        }
    }
}
