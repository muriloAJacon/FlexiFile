using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class FileRepository : Repository<Core.Entities.Postgres.File>, IFileRepository {
		public FileRepository(PostgresContext context) : base(context) {
		}
	}
}
