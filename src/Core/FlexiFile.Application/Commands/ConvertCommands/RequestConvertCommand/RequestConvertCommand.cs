using FlexiFile.Core.Interfaces.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand {
	public sealed record RequestConvertCommand : IRequest<IResultCommand> {
		public List<Guid> FileIds { get; init; } = null!;
		public int ConversionId { get; init; }
		public JsonElement? ExtraParameters { get; init; }
	}
}
