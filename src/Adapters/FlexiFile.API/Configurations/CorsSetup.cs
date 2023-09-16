using FlexiFile.Core.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FlexiFile.API.Configurations {
	public static class CorsSetup {
		public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration) {
			services.AddOptions<Cors>()
				.Bind(configuration.GetSection("Cors"))
				.ValidateOnStart();

			services.AddCors(options => {
				var corsSettings = services.BuildServiceProvider().GetRequiredService<IOptions<Cors>>().Value;

				options.AddDefaultPolicy(builder => {
					builder
						.WithOrigins(corsSettings.AllowedOrigins)
						.WithMethods(corsSettings.AllowedMethods)
						.AllowAnyHeader()
						.AllowCredentials();
				});
			});

			return services;
		}
	}
}
