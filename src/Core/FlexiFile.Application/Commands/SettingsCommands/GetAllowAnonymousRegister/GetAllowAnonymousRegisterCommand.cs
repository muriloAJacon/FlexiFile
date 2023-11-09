using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.SettingsCommands.GetAllowAnonymousRegister {
	public sealed record GetAllowAnonymousRegisterCommand : IRequest<IResultCommand> {
	}
}
