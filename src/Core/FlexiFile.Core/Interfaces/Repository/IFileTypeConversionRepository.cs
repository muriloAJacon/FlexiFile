using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface IFileTypeConversionRepository : IRepository<FileTypeConversion> {
		Task<FileTypeConversion?> GetByFromFileTypeToFileTypeAsync(int fromFileTypeId, int toFileTypeId);
	}
}
