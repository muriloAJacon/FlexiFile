using FlexiFile.Core.Enums;

namespace FlexiFile.Application.ViewModels.UserViewModels {
	public class UserViewModel {
		public Guid Id { get; set; }
		public string Name { get; set; } = null!;
		public string Email { get; set; } = null!;
		public AccessLevel AccessLevel { get; set; }
		public bool Approved { get; set; }
		public DateTime? ApprovedAt { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime LastUpdateDate { get; set; }
		public long? StorageLimit { get; set; }
	}
}
