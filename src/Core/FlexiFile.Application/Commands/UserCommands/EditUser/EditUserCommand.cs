using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.EditUser {
	public record EditUserCommand : IRequest<IResultCommand> {
		public Guid Id { get; init; }
		public string Name { get; init; } = null!;
		public string? Password { get; init; }
		public long? StorageLimit { get; init; }
		public long? HardStorageLimit { get; init; }
	}
}
