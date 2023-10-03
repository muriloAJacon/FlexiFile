namespace FlexiFile.Worker.Options {
	public static class ExtensionOptions {
		public static void ConfigureMediatR(MediatRServiceConfiguration options) {
			options.RegisterServicesFromAssemblyContaining<Program>();
			options.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("FlexiFile.Application"));
		}
	}
}
