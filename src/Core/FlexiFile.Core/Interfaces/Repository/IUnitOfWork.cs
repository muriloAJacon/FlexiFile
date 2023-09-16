namespace FlexiFile.Core.Interfaces.Repository {
	public interface IUnitOfWork : IDisposable {

		IFileConversionRepository FileConversionRepository { get; }

		IFileConversionResultRepository FileConversionResultRepository { get; }

		IFileRepository FileRepository { get; }

		IFileTypeConversionRepository FileTypeConversionRepository { get; }

		IFileTypeRepository FileTypeRepository { get; }

		ISettingRepository SettingRepository { get; }

		IUserLoginAuditRepository UserLoginAuditRepository { get; }

		IUserRefreshTokenRepository UserRefreshTokenRepository { get; }

		IUserRepository UserRepository { get; }

		Task<int> Commit();
	}
}
