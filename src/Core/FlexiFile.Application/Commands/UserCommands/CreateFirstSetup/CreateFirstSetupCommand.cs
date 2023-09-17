using FlexiFile.Core.Interfaces.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.UserCommands.CreateFirstSetup {
	public record CreateFirstSetupCommand : IRequest<IResultCommand> {
		public string Name { get; init; } = null!;
		public string Email { get; init; } = null!;
		public string Password { get; init; } = null!;
	}
}
