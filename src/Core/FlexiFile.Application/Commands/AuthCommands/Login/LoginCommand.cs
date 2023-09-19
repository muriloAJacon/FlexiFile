using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.AuthCommands.Login {
	public sealed record LoginCommand : IRequest<IResultCommand> {
		public string Email { get; init; } = null!;
		public string Password { get; init; } = null!;
	}
}
