using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.GetFirstSetup {
	public record GetFirstSetupCommand : IRequest<IResultCommand> {
	}
}
