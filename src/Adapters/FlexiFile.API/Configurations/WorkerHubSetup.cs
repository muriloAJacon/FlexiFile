using FlexiFile.Application.HubClients;
using FlexiFile.Core.OptionsBuilder;

namespace FlexiFile.API.Configurations {
	public static class WorkerHubSetup {
		public static IServiceCollection AddWorkerHub(this IServiceCollection services, Action<SignalRClientOptionsBuilder> optionsBuilderAction) {
			var optionsBuilder = new SignalRClientOptionsBuilder();
			optionsBuilderAction.Invoke(optionsBuilder);
			services.AddSingleton(provider => new ConvertHubClient(optionsBuilder, provider.GetRequiredService<ILogger<ConvertHubClient>>()));

			return services;
		}
	}
}
