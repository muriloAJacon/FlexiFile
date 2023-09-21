using FlexiFile.Core.Entities.Postgres;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface IFileRepository : IRepository<File> {
		Task<File?> GetUserFileByIdAsync(Guid id, Guid userId);
	}
}
