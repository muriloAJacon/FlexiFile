using FlexiFile.Core.Interfaces.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand {
	public sealed record RequestConvertCommand : IRequest<IResultCommand> {
		public Guid FileId { get; init; }
		public int ToTypeId { get; init; }
	}
}
