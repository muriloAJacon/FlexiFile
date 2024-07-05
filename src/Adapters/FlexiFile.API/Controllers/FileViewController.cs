using FlexiFile.Application.Security.FileAccess;
using FlexiFile.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FlexiFile.API.Controllers {
	[Route("files")]
	[ApiController]
	public class FileViewController : ControllerBase {

		private readonly FileSigningConfigurations _fileSigningConfigurations;
		private readonly FileTokenConfigurations _fileTokenConfigurations;
		private readonly ILogger<FileViewController> _logger;

		public FileViewController(FileSigningConfigurations fileSigningConfigurations, FileTokenConfigurations fileTokenConfigurations, ILogger<FileViewController> logger) {
			_fileSigningConfigurations = fileSigningConfigurations;
			_fileTokenConfigurations = fileTokenConfigurations;
			_logger = logger;
		}

		[HttpGet("{token}")]
		public IActionResult GetFile(string token, [FromQuery] bool download = false) {
			try {
				var tokenInfo = ValidateToken(token);

				string? fileName = download ? tokenInfo.FileName : null;

				return PhysicalFile(tokenInfo.FilePath, tokenInfo.FileType, fileName);
			} catch (SecurityTokenException) {
				return Unauthorized();
			} catch (Exception e) {
				_logger.LogError(e, "Failed to access file");
				return StatusCode(500);
			}
		}

		private FileParsedTokenViewModel ValidateToken(string token) {
			var handler = new JwtSecurityTokenHandler();
			var validations = new TokenValidationParameters {
				IssuerSigningKey = _fileSigningConfigurations.Key,
				ValidAudience = _fileTokenConfigurations.Audience,
				ValidIssuer = _fileTokenConfigurations.Issuer,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};

			var claims = handler.ValidateToken(token, validations, out _);

			return new FileParsedTokenViewModel {
				FileId = Guid.Parse(claims.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),
				FileName = claims.Claims.First(x => x.Type == ClaimTypes.Name).Value,
				FilePath = claims.Claims.First(x => x.Type == ClaimTypes.UserData).Value,
				FileType = claims.Claims.First(x => x.Type == ClaimTypes.System).Value
			};
		}
	}
}
