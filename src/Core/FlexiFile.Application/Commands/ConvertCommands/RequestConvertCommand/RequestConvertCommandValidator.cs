using FlexiFile.Application.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand {
	public class RequestConvertCommandValidator : AbstractValidator<RequestConvertCommand> {
		public RequestConvertCommandValidator() {
			RuleFor(x => x.FileIds)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
