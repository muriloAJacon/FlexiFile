namespace FlexiFile.Application.Security.FileAccess {
	public class FileTokenConfigurations {
		public string Audience { get; set; } = null!;

		public string Issuer { get; set; } = null!;

		public int Seconds { get; set; }

		public FileTokenConfigurations() { }

		public FileTokenConfigurations(string audience, string issuer, int seconds) {
			Audience = audience ?? throw new ArgumentNullException(nameof(audience));
			Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
			Seconds = seconds;
		}
	}
}
