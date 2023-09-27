using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.UserViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Infrastructure.Services;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.ApproveUser {
	public class ApproveUserCommandHandler : IRequestHandler<ApproveUserCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public ApproveUserCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(ApproveUserCommand request, CancellationToken cancellationToken) {

			var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
			if (user is null) {
				return ResultCommand.Unauthorized("User not found.", "userNotFound");
			}

			user.Approved = true;
			user.ApprovedAt = DateTime.UtcNow;
			user.ApprovedByUserId = _userClaimsService.Id;
			user.LastUpdateDate = DateTime.UtcNow;

			await _unitOfWork.Commit();

			return ResultCommand.Ok<User, UserViewModel>(user);
		}
	}
}
