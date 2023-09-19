using FlexiFile.Core.Enums;

namespace FlexiFile.Core.Interfaces.Services {
	public interface IUserClaimsService {
		Guid Id { get; }
		string Name { get; }
		string Email { get; }
		AccessLevel AccessLevel { get; }

		Guid? TryGetId();
		string? TryGetName();
		string? TryGetEmail();
		AccessLevel? TryGetAccessLevel ();
	}
}
