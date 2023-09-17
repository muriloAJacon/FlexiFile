using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class UserRepository : Repository<User>, IUserRepository {
		public UserRepository(PostgresContext context) : base(context) {
		}

		public async Task<bool> AnyAsync() => await Context.Users.AnyAsync();
	}
}
