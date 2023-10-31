using FlexiFile.Core.Interfaces.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand {
	public sealed record RequestConvertCommand : IRequest<IResultCommand> {
		public List<Guid> FileIds { get; init; } = null!;
		public int ConversionId { get; init; }
	}
}
