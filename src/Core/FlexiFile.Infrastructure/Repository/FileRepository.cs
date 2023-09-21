using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Infrastructure.Repository {
	public class FileRepository : Repository<File>, IFileRepository {
		public FileRepository(PostgresContext context) : base(context) {
		}

		public async Task<File?> GetUserFileByIdAsync(Guid id, Guid userId) => await Context.Files.Include(x => x.Type).FirstOrDefaultAsync(x => x.Id == id && x.OwnedByUserId == userId);
	}
}
