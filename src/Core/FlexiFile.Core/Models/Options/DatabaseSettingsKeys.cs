namespace FlexiFile.Core.Models.Options {
	public sealed record DatabaseSettingsKeys {
		public string AllowAnonymousRegisterKey { get; init; } = null!;
		public string GlobalMaximumFileSizeKey { get; init; } = null!;
	}
}
