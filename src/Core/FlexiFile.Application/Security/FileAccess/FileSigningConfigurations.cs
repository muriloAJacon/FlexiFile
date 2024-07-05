using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography;

namespace FlexiFile.Application.Security.FileAccess {
	public class FileSigningConfigurations {
		public SecurityKey Key { get; }

		public SigningCredentials SigningCredentials { get; }

		public FileSigningConfigurations() {
			using RSACryptoServiceProvider provider = new();
			provider.ImportFromPem(File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Security/FilePrivateKey.pem"));

			Key = new RsaSecurityKey(provider.ExportParameters(true));

			SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha512Signature);
		}

		public FileSigningConfigurations(string pemFile) {
			using RSACryptoServiceProvider provider = new();
			provider.ImportFromPem(pemFile);

			Key = new RsaSecurityKey(provider.ExportParameters(true));

			SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha512Signature);
		}
	}
}
