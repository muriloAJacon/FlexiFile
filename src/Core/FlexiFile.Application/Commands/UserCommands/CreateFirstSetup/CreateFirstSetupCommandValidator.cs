using FlexiFile.Application.Results;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.UserCommands.CreateFirstSetup {
	public class CreateFirstSetupCommandValidator : AbstractValidator<CreateFirstSetupCommand> {
		public CreateFirstSetupCommandValidator() {
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
