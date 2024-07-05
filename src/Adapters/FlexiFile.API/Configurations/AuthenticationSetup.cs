using FlexiFile.Application.Security;
using FlexiFile.Application.Security.FileAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FlexiFile.API.Configurations {
	public static class AuthenticationSetup {
		public static void AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration) {
			SigningConfigurations signingConfigurations = new();
			services.AddSingleton(signingConfigurations);

			TokenConfigurations tokenConfigurations = new();
			new ConfigureFromConfigurationOptions<TokenConfigurations>(
				configuration.GetSection("TokenConfigurations"))
					.Configure(tokenConfigurations);
			services.AddSingleton(tokenConfigurations);

			services.AddAuthentication(x => {
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x => {
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters {
					IssuerSigningKey = signingConfigurations.Key,
					ValidAudience = tokenConfigurations.Audience,
					ValidIssuer = tokenConfigurations.Issuer,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});
		}

		public static void AddFileBearerAuthentication(this IServiceCollection services, IConfiguration configuration) {
			FileSigningConfigurations fileSigningConfigurations = new();
			services.AddSingleton(fileSigningConfigurations);

			FileTokenConfigurations fileTokenConfigurations = new();
			new ConfigureFromConfigurationOptions<FileTokenConfigurations>(
								configuration.GetSection("FileTokenConfigurations"))
					.Configure(fileTokenConfigurations);
			services.AddSingleton(fileTokenConfigurations);
		}
	}
}
