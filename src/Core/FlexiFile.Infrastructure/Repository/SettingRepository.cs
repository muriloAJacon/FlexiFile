using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Models.Options;
using FlexiFile.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Infrastructure.Repository {
	public class SettingRepository : Repository<Setting>, ISettingRepository {
		private readonly DatabaseSettingsKeys _keys;

		public SettingRepository(PostgresContext context, DatabaseSettingsKeys keys) : base(context) {
			_keys = keys;
		}

		public async Task<bool> GetAllowAnonymousRegister() {
			var setting = await Context.Settings.SingleAsync(x => x.Id == _keys.AllowAnonymousRegisterKey);
			return Convert.ToBoolean(setting.Value);
		}

		public async Task<long> GetGlobalMaximumFileSize() {
			var setting = await Context.Settings.SingleAsync(x => x.Id == _keys.GlobalMaximumFileSizeKey);
			return Convert.ToInt64(setting.Value);
		}
	}
}
