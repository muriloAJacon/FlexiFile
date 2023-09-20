using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.AuthCommands.RequestRefreshToken {
	public sealed record RequestRefreshTokenCommand : IRequest<IResultCommand> {
		public Guid RefreshToken { get; set; }
	}
}
