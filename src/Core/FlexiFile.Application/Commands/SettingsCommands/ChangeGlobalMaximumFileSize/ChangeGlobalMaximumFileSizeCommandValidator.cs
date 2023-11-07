using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.SettingsCommands.ChangeGlobalMaximumFileSize {
	public class ChangeGlobalMaximumFileSizeCommandValidator : AbstractValidator<ChangeGlobalMaximumFileSizeCommand> {
		public ChangeGlobalMaximumFileSizeCommandValidator() {
			RuleFor(x => x.MaxFileSize)
				.GreaterThan(0).WithMessage("The value must be greater than 0");
		}
	}
}
