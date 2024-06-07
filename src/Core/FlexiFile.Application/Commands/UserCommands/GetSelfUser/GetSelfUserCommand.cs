using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.GetSelfUser {
	public sealed record GetSelfUserCommand : IRequest<IResultCommand> {
	}
}
