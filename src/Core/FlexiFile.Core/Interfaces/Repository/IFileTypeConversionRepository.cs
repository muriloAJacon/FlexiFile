using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface IFileTypeConversionRepository : IRepository<FileTypeConversion> {
		Task<List<FileTypeConversion>> GetAvailableConversions(string fromMimeType);
		Task<FileTypeConversion?> GetByFromFileTypeToFileTypeAsync(int fromFileTypeId, int toFileTypeId);
		Task<FileTypeConversion?> GetByIdAsync(int id);
	}
}
