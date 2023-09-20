using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class UserRefreshTokenRepository : Repository<UserRefreshToken>, IUserRefreshTokenRepository {
		public UserRefreshTokenRepository(PostgresContext context) : base(context) {
		}

		public async Task<UserRefreshToken?> GetByRefreshTokenAsync(Guid refreshToken) => await Context.UserRefreshTokens.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == refreshToken);
	}
}
