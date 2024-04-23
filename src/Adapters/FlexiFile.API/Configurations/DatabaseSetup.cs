using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using FlexiFile.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.API.Configurations {
	public static class DatabaseSetup {
		public static void AddPostgres(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env) {
			services.AddDbContext<PostgresContext>(options => {
				options.UseNpgsql(configuration.GetConnectionString("Postgres"), x => x.MigrationsAssembly("FlexiFile.API"));
				options.EnableSensitiveDataLogging(env.IsDevelopment());
			});
		}

		public static IServiceCollection AddRepositories(this IServiceCollection services) {
			services.AddTransient<IUnitOfWork, UnitOfWork>();

			return services;
		}

		public static void UseMigrations(this WebApplication app) {
			using var scope = app.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<PostgresContext>();

			if (context.Database.GetPendingMigrations().Any()) {
				context.Database.Migrate();
			}
		}
	}
}
