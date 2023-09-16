using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class SettingRepository : Repository<Setting>, ISettingRepository {
		public SettingRepository(PostgresContext context) : base(context) {
		}
	}
}
