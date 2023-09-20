using BCrypt.Net;

namespace FlexiFile.Infrastructure.Services {
	public static class PasswordService {
		public static string HashPassword(string password) {
			return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA384);
		}

		public static bool VerifyHashedPassword(string providedPassword, string hashedPassword) {
			return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, hashedPassword, HashType.SHA384);
		}
	}
}
