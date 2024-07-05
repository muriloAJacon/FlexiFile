using FlexiFile.Application.ViewModels;
using FlexiFile.Core.Enums;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FlexiFile.Application.Security.FileAccess {
	public static class FileTokenService {
		public static FileBearerTokenViewModel GenerateToken(Guid fileId, string filePath, FileType fileType, FileSigningConfigurations fileSigningConfigurations, FileTokenConfigurations fileTokenConfigurations) {
			DateTime creationDate = DateTime.UtcNow;
			DateTime expirationDate = creationDate.AddSeconds(fileTokenConfigurations.Seconds);

			JwtSecurityTokenHandler tokenHandler = new();
			SecurityTokenDescriptor tokenDescriptor = new() {
				Subject = new ClaimsIdentity(new Claim[] {
					new(ClaimTypes.NameIdentifier, fileId.ToString()),
					new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
					new(ClaimTypes.Name, filePath),
					new(ClaimTypes.UserData, fileType.ToString()),
				}),
				Issuer = fileTokenConfigurations.Issuer,
				Audience = fileTokenConfigurations.Audience,
				Expires = expirationDate,
				NotBefore = creationDate,
				SigningCredentials = fileSigningConfigurations.SigningCredentials
			};

			SecurityToken createToken = tokenHandler.CreateToken(tokenDescriptor);
			string token = tokenHandler.WriteToken(createToken);

			return new FileBearerTokenViewModel {
				ExpiresAt = expirationDate,
				Token = token
			};
		}
	}
}
