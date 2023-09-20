using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface IUserRepository : IRepository<User> {
		Task<bool> AnyAsync();
		Task<User?> GetByEmailAsync(string email);
	}
}
