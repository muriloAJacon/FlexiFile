using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class FileConversionResultRepository : Repository<FileConversionResult>, IFileConversionResultRepository {
		public FileConversionResultRepository(PostgresContext context) : base(context) {
		}

		public async Task<FileConversionResult?> GetUserFileByIdAsync(Guid id, Guid userId) => await Context.FileConversionResults.Include(x => x.FileConversion)
																															.ThenInclude(x => x.User)
																															.FirstOrDefaultAsync(x => x.Id == id && x.FileConversion.UserId == userId);
	}
}
