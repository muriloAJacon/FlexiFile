using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Services;

namespace FlexiFile.Infrastructure.Services {
	public class ValidateUserStorageService : IValidateUserStorageService {

		public bool ValidateStorageForConvertedFile(User user, long fileSize) {
			if (user.HardStorageLimit is not null && fileSize + user.StorageUsed > user.HardStorageLimit) {
				return false;
			}

			return true;
		}

		public bool ValidateStorageForNewFile(User user, long fileSize) {
			if (user.StorageLimit is not null && fileSize + user.StorageUsed > user.StorageLimit) {
				return false;
			}

			return true;
		}
	}
}
