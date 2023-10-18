using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.SettingsCommands.ChangeAllowAnonymousRegister {
	public sealed record ChangeAllowAnonymousRegisterCommand : IRequest<IResultCommand> {
		public bool AllowAnonymousRegister { get; init; }
	}
}
