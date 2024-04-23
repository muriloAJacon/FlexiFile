using FlexiFile.Application.Results;
using FlexiFile.Application.Security;
using FlexiFile.Application.ViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FlexiFile.Application.Commands.AuthCommands.Login {
	public class LoginCommandHandler : IRequestHandler<LoginCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly SigningConfigurations _signingConfigurations;
		private readonly TokenConfigurations _tokenConfigurations;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LoginCommandHandler(IUnitOfWork unitOfWork, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, IHttpContextAccessor httpContextAccessor) {
			_unitOfWork = unitOfWork;
			_signingConfigurations = signingConfigurations;
			_tokenConfigurations = tokenConfigurations;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IResultCommand> Handle(LoginCommand request, CancellationToken cancellationToken) {
			var user = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);

			if (user is null) {
				return ResultCommand.Unauthorized("Invalid email or password.", "invalidCredentials");
			}

			bool loginSuccessful = false;

			try {

				if (!PasswordService.VerifyHashedPassword(request.Password, user.Password)) {
					return ResultCommand.Unauthorized("Invalid email or password.", "invalidCredentials");
				}

				if (!user.Approved) {
					return ResultCommand.Unauthorized("Your account is not approved yet.", "notApproved");
				}

				loginSuccessful = true;
				var token = TokenService.GenerateToken(user, _signingConfigurations, _tokenConfigurations);

				var refreshToken = new UserRefreshToken {
					Id = token.RefreshToken,
					User = user,
					CreatedAt = DateTime.UtcNow,
					ExpiresAt = token.RefreshTokenExpiresAt,
				};

				_unitOfWork.UserRefreshTokenRepository.Add(refreshToken);
				await _unitOfWork.Commit();

				return ResultCommand.Ok<BearerTokenViewModel, BearerTokenViewModel>(token);
			} catch {
				loginSuccessful = false;
				throw;
			} finally {
				await RegisterLoginAudit(user, loginSuccessful);
			}

		}

		private async Task<UserLoginAudit> RegisterLoginAudit(User user, bool successful) {
			var loginAudit = new UserLoginAudit {
				Id = Guid.NewGuid(),
				User = user,
				Timestamp = DateTime.UtcNow,
				Successful = successful,
				SourceIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "",
				SourceUserAgent = _httpContextAccessor.HttpContext?.Request.Headers.UserAgent.ToString() ?? ""
			};

			_unitOfWork.UserLoginAuditRepository.Add(loginAudit);
			await _unitOfWork.Commit();

			return loginAudit;
		}
	}
}
