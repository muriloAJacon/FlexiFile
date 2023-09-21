using FlexiFile.Core.Interfaces.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FlexiFile.Application.Commands.FileCommands.FileUpload {
	public sealed record FileUploadCommand : IRequest<IResultCommand> {
		public Guid FileId { get; init; }
		public IFormFile File { get; init; } = null!;
	}
}
