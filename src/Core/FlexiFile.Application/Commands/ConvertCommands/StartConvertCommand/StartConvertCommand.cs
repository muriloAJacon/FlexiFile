using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.ConvertCommands.StartConvertCommand {
	public sealed record StartConvertCommand : IRequest {
		public Guid ConversionId { get; init; }
	}
}
