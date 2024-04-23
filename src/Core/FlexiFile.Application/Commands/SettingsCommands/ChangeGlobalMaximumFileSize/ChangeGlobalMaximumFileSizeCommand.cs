using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.SettingsCommands.ChangeGlobalMaximumFileSize {
	public sealed record ChangeGlobalMaximumFileSizeCommand : IRequest<IResultCommand> {
		public long MaxFileSize { get; init; }
	}
}
