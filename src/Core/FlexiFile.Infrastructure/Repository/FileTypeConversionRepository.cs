using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class FileTypeConversionRepository : Repository<FileTypeConversion>, IFileTypeConversionRepository {
		public FileTypeConversionRepository(PostgresContext context) : base(context) {
		}
	}
}
