using FlexiFile.Application.Results;
using FluentValidation;

namespace FlexiFile.Application.Commands.FileCommands.StartFileUpload {
	public class StartFileUploadValidator : AbstractValidator<StartFileUploadCommand> {
		public StartFileUploadValidator() {
			RuleFor(x => x.FileName)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
