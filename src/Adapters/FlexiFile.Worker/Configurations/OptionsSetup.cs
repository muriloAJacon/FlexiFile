using FlexiFile.Core.Models.Options;

namespace FlexiFile.Worker.Configurations {
	public static class OptionsSetup {
		public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration) {
			services.AddOptions<DatabaseSettingsKeys>()
				.Bind(configuration.GetSection("DatabaseSettingsKeys"))
				.ValidateOnStart();

			return services;
		}
	}
}
