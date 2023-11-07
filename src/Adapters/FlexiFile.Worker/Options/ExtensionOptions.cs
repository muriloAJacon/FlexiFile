using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;

namespace FlexiFile.Worker.Options {
	public static class ExtensionOptions {
		public static void ConfigureMediatR(MediatRServiceConfiguration options) {
			options.RegisterServicesFromAssemblyContaining<Program>();
			options.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("FlexiFile.Application"));
		}

		public static void ConfigureJson(JsonHubProtocolOptions options) {
			options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			options.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		}
	}
}
