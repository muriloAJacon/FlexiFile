using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class FileTypeRepository : Repository<FileType>, IFileTypeRepository {
		public FileTypeRepository(PostgresContext context) : base(context) {
		}

		public async Task<FileType?> GetByMimeTypeAsync(string mimeType) => await Context.FileTypes.AsNoTracking().FirstOrDefaultAsync(x => x.MimeType == mimeType);
	}
}
