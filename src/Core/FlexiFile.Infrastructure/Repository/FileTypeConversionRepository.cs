using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class FileTypeConversionRepository : Repository<FileTypeConversion>, IFileTypeConversionRepository {
		public FileTypeConversionRepository(PostgresContext context) : base(context) {
		}

		public async Task<FileTypeConversion?> GetByFromFileTypeToFileTypeAsync(int fromFileTypeId, int toFileTypeId) {
			return await Context.FileTypeConversions.FirstOrDefaultAsync(x => x.FromTypeId == fromFileTypeId && x.ToTypeId == toFileTypeId && x.IsActive);
		}
	}
}
