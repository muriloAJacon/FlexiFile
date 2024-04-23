using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.GetUsers {
	public sealed record GetUsersCommand : IRequest<IResultCommand> {
	}
}
