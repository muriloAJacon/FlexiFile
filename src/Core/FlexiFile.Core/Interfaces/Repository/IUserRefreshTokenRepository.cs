using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface IUserRefreshTokenRepository : IRepository<UserRefreshToken> {
		Task<UserRefreshToken?> GetByRefreshTokenAsync(Guid refreshToken);
	}
}
