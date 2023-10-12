using FlexiFile.Core.Enums;
using MediatR;

namespace FlexiFile.Application.Commands.ConvertCommands.UpdateConvertProgressCommand {
	public class UpdateConvertProgressCommand : IRequest {
		public Guid ConversionId { get; set; }
		public ConvertStatus ConvertStatus { get; set; }
		public double? PercentageComplete { get; set; }
	}
}
