using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.FileCommands.CreateFileAuthToken {
	public sealed record CreateFileAuthTokenCommand : IRequest<IResultCommand> {
		public Guid FileId { get; set; }
		public FileType FileType { get; set; }
	}
}
