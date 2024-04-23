using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.FileCommands.GetFileInfo {
	public sealed record GetFileInfoCommand : IRequest<IResultCommand> {
		public Guid Id { get; init; }

		public GetFileInfoCommand(Guid id) {
			Id = id;
		}
	}
}
