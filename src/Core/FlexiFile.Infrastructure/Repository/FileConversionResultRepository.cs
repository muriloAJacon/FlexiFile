using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class FileConversionResultRepository : Repository<FileConversionResult>, IFileConversionResultRepository {
		public FileConversionResultRepository(PostgresContext context) : base(context) {
		}
	}
}
