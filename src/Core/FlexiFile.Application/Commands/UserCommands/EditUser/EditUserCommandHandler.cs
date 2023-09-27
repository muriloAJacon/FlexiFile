using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.UserViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Infrastructure.Services;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.EditUser {
	public class EditUserCommandHandler : IRequestHandler<EditUserCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public EditUserCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<IResultCommand> Handle(EditUserCommand request, CancellationToken cancellationToken) {

			var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
			if (user is null) {
				return ResultCommand.Unauthorized("User not found.", "userNotFound");
			}

			user.Name = request.Name;
			user.LastUpdateDate = DateTime.UtcNow;

			if (request.Password is not null) {
				user.Password = PasswordService.HashPassword(request.Password);
			}

			await _unitOfWork.Commit();

			return ResultCommand.Ok<User, UserViewModel>(user);
		}
	}
}
