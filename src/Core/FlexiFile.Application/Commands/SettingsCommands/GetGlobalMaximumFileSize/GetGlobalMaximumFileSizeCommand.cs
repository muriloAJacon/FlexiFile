using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.SettingsCommands.GetGlobalMaximumFileSize {
	public sealed record GetGlobalMaximumFileSizeCommand : IRequest<IResultCommand> {
	}
}
