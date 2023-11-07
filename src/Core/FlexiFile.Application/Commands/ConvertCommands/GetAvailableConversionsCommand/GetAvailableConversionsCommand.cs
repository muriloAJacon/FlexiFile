using FlexiFile.Core.Interfaces.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FlexiFile.Application.Commands.ConvertCommands.GetAvailableConversionsCommand {
	public sealed record GetAvailableConversionsCommand : IRequest<IResultCommand> {
		public string FromMimeType { get; init; } = null!;

		public GetAvailableConversionsCommand(string fromMimeType) {
			FromMimeType = fromMimeType;
		}
	}
}
