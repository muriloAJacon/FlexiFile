using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Infrastructure.Repository {
	public class FileRepository : Repository<File>, IFileRepository {
		public FileRepository(PostgresContext context) : base(context) {
		}

		public async Task<File?> GetUserFileByIdAsync(Guid id, Guid userId) => await Context.Files.Include(x => x.OwnedByUser)
																							.Include(x => x.Type)
																							.Include(x => x.FileConversionOrigins)
																							.ThenInclude(x => x.FileConversion)
																							.ThenInclude(x => x.FileConversionResults)
																							.FirstOrDefaultAsync(x => x.Id == id && x.OwnedByUserId == userId);

		public async Task<List<File>> GetUserFilesByIdAsync(List<Guid> ids, Guid userId) => await Context.Files.Include(x => x.Type)
																										 .Include(x => x.FileConversionOrigins)
																										 .ThenInclude(x => x.FileConversion)
																										 .ThenInclude(x => x.FileConversionResults)
																										 .Where(x => ids.Contains(x.Id) && x.OwnedByUserId == userId)
																										 .ToListAsync();

		public async Task<List<File>> GetUploadedUserFilesWithExceptionsAsync(List<Guid> ids, Guid userId) => await Context.Files.Include(x => x.Type)
																										 .Include(x => x.FileConversionOrigins)
																										 .ThenInclude(x => x.FileConversion)
																										 .ThenInclude(x => x.FileConversionResults)
																										 .Where(x => !ids.Contains(x.Id) && x.OwnedByUserId == userId && x.FinishedUpload)
																										 .OrderByDescending(x => x.SubmittedAt)
																										 .AsSplitQuery()
																										 .ToListAsync();

		public new async Task<File?> GetByIdAsync(Guid id) => await Context.Files.Include(x => x.Type).SingleOrDefaultAsync(x => x.Id == id);
	}
}
