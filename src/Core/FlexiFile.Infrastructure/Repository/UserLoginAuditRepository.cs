using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class UserLoginAuditRepository : Repository<UserLoginAudit>, IUserLoginAuditRepository {
		public UserLoginAuditRepository(PostgresContext context) : base(context) {
		}
	}
}
