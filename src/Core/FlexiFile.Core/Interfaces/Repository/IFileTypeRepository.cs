using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface IFileTypeRepository : IRepository<FileType> {
		Task<FileType?> GetByMimeTypeAsync(string mimeType);
	}
}
