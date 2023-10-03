using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class FileConversionRepository : Repository<FileConversion>, IFileConversionRepository {
		public FileConversionRepository(PostgresContext context) : base(context) {
		}

		public new async Task<FileConversion?> GetByIdAsync(Guid id) {
			return await Context.FileConversions.Include(x => x.FileTypeConversion)
									   .ThenInclude(x => x.FromType)
									   .Include(x => x.FileTypeConversion)
									   .ThenInclude(x => x.ToType)
									   .Include(x => x.File)
									   .FirstOrDefaultAsync(x => x.Id == id);
		}
	}
}
