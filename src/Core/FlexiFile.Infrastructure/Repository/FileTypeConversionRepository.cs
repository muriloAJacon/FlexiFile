using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class FileTypeConversionRepository : Repository<FileTypeConversion>, IFileTypeConversionRepository {
		public FileTypeConversionRepository(PostgresContext context) : base(context) {
		}

		public async Task<FileTypeConversion?> GetByIdAsync(int id) => await Context.FileTypeConversions.SingleOrDefaultAsync(x => x.Id == id && x.IsActive);

		public async Task<FileTypeConversion?> GetByFromFileTypeToFileTypeAsync(int fromFileTypeId, int toFileTypeId) {
			return await Context.FileTypeConversions.FirstOrDefaultAsync(x => x.FromTypeId == fromFileTypeId && x.ToTypeId == toFileTypeId && x.IsActive);
		}

		public async Task<List<FileTypeConversion>> GetAvailableConversions(string fromMimeType) {
			return await Context.FileTypeConversions.Include(x => x.FromType).Include(x => x.ToType).Where(x => x.FromType.MimeTypes.Contains(fromMimeType) && x.IsActive).ToListAsync();
		}
	}
}
