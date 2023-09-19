using FlexiFile.Application.Results;
using FluentValidation;

namespace FlexiFile.Application.Commands.UserCommands.CreateUser {
	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand> {
		public CreateUserCommandValidator() {
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MaximumLength(250).WithMessage(ValidatorsModelErrorMessages.MaxLength);

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.EmailAddress().WithMessage(ValidatorsModelErrorMessages.Email);

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage(ValidatorsModelErrorMessages.NullOrEmpty)
				.MinimumLength(8).WithMessage(ValidatorsModelErrorMessages.MinLength)
				.Matches("[A-Z]").WithMessage(ValidatorsModelErrorMessages.PasswordCapitalLetters)
				.Matches("[0-9]").WithMessage(ValidatorsModelErrorMessages.PasswordNumbers);
		}
	}
}
