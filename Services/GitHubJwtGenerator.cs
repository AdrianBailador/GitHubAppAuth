using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace GitHubAppAuth.Services
{
    public class GitHubJwtGenerator
    {
        private readonly string _privateKey;
        private readonly int _appId;

        public GitHubJwtGenerator(string privateKey, int appId)
        {
            _privateKey = privateKey;
            _appId = appId;
        }

        public string GenerateJwt()
        {
            using var rsa = RSA.Create();
            rsa.ImportFromPem(_privateKey.ToCharArray());

            var securityKey = new RsaSecurityKey(rsa);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            var now = DateTimeOffset.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _appId.ToString(),
                IssuedAt = now.UtcDateTime,
                Expires = now.AddMinutes(10).UtcDateTime,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
