using FlexiFile.Application.Results;
using FluentValidation;

namespace FlexiFile.Application.Commands.UserCommands.EditUser {
	public class EditUserCommandValidator : AbstractValidator<EditUserCommand> {
		public EditUserCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(250).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Password)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers);
		}
	}
}
