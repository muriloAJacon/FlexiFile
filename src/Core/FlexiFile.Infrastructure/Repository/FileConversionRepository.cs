using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class FileConversionRepository : Repository<FileConversion>, IFileConversionRepository {
		public FileConversionRepository(PostgresContext context) : base(context) {
		}
	}
}
