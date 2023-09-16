using FlexiFile.Core.Entities.Postgres;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace FlexiFile.Application.Security {
	public static class TokenService {
		public static string GenerateToken(User user, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations) {
			DateTime creationDate = DateTime.UtcNow;
			DateTime expirationDate = creationDate + TimeSpan.FromSeconds(tokenConfigurations.Seconds);
			TimeSpan finalExpiration = TimeSpan.FromSeconds(tokenConfigurations.FinalExpiration);

			JwtSecurityTokenHandler tokenHandler = new();
			SecurityTokenDescriptor tokenDescriptor = new() {
				Subject = new ClaimsIdentity(new Claim[] {
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Role, user.AccessLevel.ToString())
				}),
				Issuer = tokenConfigurations.Issuer,
				Audience = tokenConfigurations.Audience,
				Expires = expirationDate,
				NotBefore = creationDate,
				SigningCredentials = signingConfigurations.SigningCredentials
			};

			SecurityToken createToken = tokenHandler.CreateToken(tokenDescriptor);
			string token = tokenHandler.WriteToken(createToken);

			return token;
		}
	}
}
