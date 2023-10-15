namespace FlexiFile.Core.OptionsBuilder {

	public class SignalRClientOptionsBuilder {
		public string? ConnectionString { get; set; }

		public SignalRClientOptionsBuilder WithConnectionString(string connectionString) {
			ConnectionString = connectionString;
			return this;
		}
	}
}
