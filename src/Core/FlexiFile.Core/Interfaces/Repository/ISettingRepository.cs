using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Repository {
	public interface ISettingRepository : IRepository<Setting> {
		Task<bool> GetAllowAnonymousRegister();
		Task<long> GetGlobalMaximumFileSize();
	}
}
