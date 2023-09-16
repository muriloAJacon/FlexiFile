using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;

namespace FlexiFile.Infrastructure.Repository {
	public class UserRefreshTokenRepository : Repository<UserRefreshToken>, IUserRefreshTokenRepository {
		public UserRefreshTokenRepository(PostgresContext context) : base(context) {
		}
	}
}
