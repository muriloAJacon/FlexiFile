namespace FlexiFile.Application.ViewModels {
	public class BearerTokenViewModel {
		public string Token { get; set; } = null!;
		public Guid RefreshToken { get; set; }
		public DateTime ExpiresAt { get; set; }
		public DateTime RefreshTokenExpiresAt { get; set; }
	}
}
