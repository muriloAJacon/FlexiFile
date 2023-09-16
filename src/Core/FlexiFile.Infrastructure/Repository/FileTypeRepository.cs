using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class FileTypeRepository : Repository<FileType>, IFileTypeRepository {
		public FileTypeRepository(PostgresContext context) : base(context) {
		}
	}
}
