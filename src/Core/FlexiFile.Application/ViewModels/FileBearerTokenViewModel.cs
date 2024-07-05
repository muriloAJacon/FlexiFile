namespace FlexiFile.Application.ViewModels {
	public class FileBearerTokenViewModel {
		public string Token { get; set; } = null!;
		public DateTime ExpiresAt { get; set; }
	}
}
