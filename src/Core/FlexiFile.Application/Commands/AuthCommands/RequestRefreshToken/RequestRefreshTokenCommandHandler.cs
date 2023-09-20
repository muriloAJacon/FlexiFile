using FlexiFile.Application.Results;
using FlexiFile.Application.Security;
using FlexiFile.Application.ViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.AuthCommands.RequestRefreshToken {
	public class RequestRefreshTokenCommandHandler : IRequestHandler<RequestRefreshTokenCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly SigningConfigurations _signingConfigurations;
		private readonly TokenConfigurations _tokenConfigurations;

		public RequestRefreshTokenCommandHandler(IUnitOfWork unitOfWork, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations) {
			_unitOfWork = unitOfWork;
			_signingConfigurations = signingConfigurations;
			_tokenConfigurations = tokenConfigurations;
		}

		public async Task<IResultCommand> Handle(RequestRefreshTokenCommand request, CancellationToken cancellationToken) {
			var refreshToken = await _unitOfWork.UserRefreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
			if (refreshToken is null) {
				return ResultCommand.Unauthorized("Invalid refresh token.", "invalidToken");
			}

			_unitOfWork.UserRefreshTokenRepository.Remove(refreshToken);
			await _unitOfWork.Commit();

			if (refreshToken.ExpiresAt < DateTime.UtcNow) {
				return ResultCommand.Unauthorized("Expired refresh token.", "expiredToken");
			}

			var user = refreshToken.User;
			if (!user.Approved) {
				return ResultCommand.Unauthorized("Invalid user for refresh token.", "invalidUser");
			}

			var token = TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations);

			var newRefreshToken = new UserRefreshToken {
				Id = token.RefreshToken,
				User = user,
				CreatedAt = DateTime.UtcNow,
				ExpiresAt = DateTime.UtcNow.AddSeconds(_tokenConfigurations.FinalExpiration),
			};

			_unitOfWork.UserRefreshTokenRepository.Add(newRefreshToken);
			await _unitOfWork.Commit();

			return ResultCommand.Ok<BearerTokenViewModel, BearerTokenViewModel>(token);
		}
	}
}
