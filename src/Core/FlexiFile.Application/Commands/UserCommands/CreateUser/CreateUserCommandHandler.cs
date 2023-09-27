﻿using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.UserViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Infrastructure.Services;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.CreateUser {
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public CreateUserCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(CreateUserCommand request, CancellationToken cancellationToken) {

			Guid? createdByUser = _userClaimsService.TryGetId();

			if (createdByUser is null) {
				// TODO: CHECK IF SETTING FOR ANONYMOUS CREATION IS ENABLED
			} else if (_userClaimsService.AccessLevel < AccessLevel.Admin) {
				return ResultCommand.Forbidden("You do not have access to perform this action.", "permissionRequired");
			}

			var sameEmail = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
			if (sameEmail is not null) {
				return ResultCommand.Unauthorized("A user with this email already exists.", "duplicateEmail");
			}

			var user = new User {
				Id = Guid.NewGuid(),
				Name = request.Name,
				Email = request.Email,
				Password = PasswordService.HashPassword(request.Password),
				AccessLevel = AccessLevel.User,
				CreationDate = DateTime.UtcNow,
				LastUpdateDate = DateTime.UtcNow,
			};

			if (createdByUser is not null) {
				user.CreatedByUserId = createdByUser;
				user.Approved = true;
				user.ApprovedAt = DateTime.UtcNow;
				user.ApprovedByUserId = createdByUser;
			}

			_unitOfWork.UserRepository.Add(user);
			await _unitOfWork.Commit();

			return ResultCommand.Ok<User, UserViewModel>(user);
		}
	}
}
