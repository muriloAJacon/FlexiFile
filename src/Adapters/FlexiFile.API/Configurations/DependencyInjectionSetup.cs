using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Infrastructure.Services;

namespace FlexiFile.API.Configurations {
	public static class DependencyInjectionSetup {
		public static void AddDependencyInjection(this IServiceCollection services) {
			services.AddTransient<IUserClaimsService, UserClaimsService>();
			services.AddTransient<IFileService, FileService>();
		}
	}
}
