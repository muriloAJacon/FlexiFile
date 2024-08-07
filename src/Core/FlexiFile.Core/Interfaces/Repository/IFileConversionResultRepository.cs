using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface IFileConversionResultRepository : IRepository<FileConversionResult> {
		Task<FileConversionResult?> GetUserFileByIdAsync(Guid id, Guid userId);
	}
}
