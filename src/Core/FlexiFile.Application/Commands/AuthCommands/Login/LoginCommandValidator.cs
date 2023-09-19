using FlexiFile.Application.Results;
using FluentValidation;

namespace FlexiFile.Application.Commands.AuthCommands.Login {
	public class LoginCommandValidator : AbstractValidator<LoginCommand> {
		public LoginCommandValidator() {
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty);
		}
	}
}
