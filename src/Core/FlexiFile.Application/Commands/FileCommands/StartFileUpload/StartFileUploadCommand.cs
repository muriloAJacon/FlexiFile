using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.FileCommands.StartFileUpload {
	public sealed record StartFileUploadCommand : IRequest<IResultCommand> {
		public string MimeType { get; init; } = null!;
		public string FileName { get; init; } = null!;
		public long FileSize { get; init; }
	}
}
