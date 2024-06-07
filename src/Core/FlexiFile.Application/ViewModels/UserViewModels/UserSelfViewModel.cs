namespace FlexiFile.Application.ViewModels.UserViewModels {
	public class UserSelfViewModel {
		public string Name { get; set; } = null!;
		public long StorageUsed { get; set; }
		public long? StorageLimit { get; set; }
		public long? HardStorageLimit { get; set; }
	}
}
