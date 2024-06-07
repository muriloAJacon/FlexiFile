using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Services {
	public interface IValidateUserStorageService {
		bool ValidateStorageForNewFile(User user, long fileSize);

		bool ValidateStorageForConvertedFile(User user, long fileSize);
	}
}
