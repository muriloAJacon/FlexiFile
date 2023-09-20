using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.CreateUser {
	public record CreateUserCommand : IRequest<IResultCommand> {
		public string Name { get; init; } = null!;
		public string Email { get; init; } = null!;
		public string Password { get; init; } = null!;
	}
}
