using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.ApproveUser {
	public record ApproveUserCommand : IRequest<IResultCommand> {
		public Guid Id { get; init; }
	}
}
