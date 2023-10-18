using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Models.Options;
using FlexiFile.Infrastructure.Context;
using Microsoft.Extensions.Options;

namespace FlexiFile.Infrastructure.Repository {
	public class UnitOfWork : IUnitOfWork {

		public IFileConversionRepository FileConversionRepository { get; private set; }

		public IFileConversionResultRepository FileConversionResultRepository { get; private set; }

		public IFileRepository FileRepository { get; private set; }

		public IFileTypeConversionRepository FileTypeConversionRepository { get; private set; }

		public IFileTypeRepository FileTypeRepository { get; private set; }

		public ISettingRepository SettingRepository { get; private set; }

		public IUserLoginAuditRepository UserLoginAuditRepository { get; private set; }

		public IUserRefreshTokenRepository UserRefreshTokenRepository { get; private set; }

		public IUserRepository UserRepository { get; private set; }


		private readonly PostgresContext _context;

		public UnitOfWork(PostgresContext context, IOptions<DatabaseSettingsKeys> settingsKeys) {
			_context = context;
			FileConversionRepository = new FileConversionRepository(_context);
			FileConversionResultRepository = new FileConversionResultRepository(_context);
			FileRepository = new FileRepository(_context);
			FileTypeConversionRepository = new FileTypeConversionRepository(_context);
			FileTypeRepository = new FileTypeRepository(_context);
			SettingRepository = new SettingRepository(_context, settingsKeys.Value);
			UserLoginAuditRepository = new UserLoginAuditRepository(_context);
			UserRefreshTokenRepository = new UserRefreshTokenRepository(_context);
			UserRepository = new UserRepository(_context);
		}

		public async Task<int> Commit() {
			return await _context.SaveChangesAsync();
		}

		public async void Dispose() {
			await _context.DisposeAsync();
			GC.SuppressFinalize(this);
		}
	}
}
