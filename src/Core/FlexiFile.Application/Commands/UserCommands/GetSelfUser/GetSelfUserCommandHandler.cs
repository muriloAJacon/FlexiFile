using FlexiFile.Application.Commands.UserCommands.GetUsers;
using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.UserViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.UserCommands.GetSelfUser {

	public class GetSelfUserCommandHandler : IRequestHandler<GetSelfUserCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public GetSelfUserCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(GetSelfUserCommand request, CancellationToken cancellationToken) {

			var user = await _unitOfWork.UserRepository.GetByIdAsync(_userClaimsService.Id) ?? throw new Exception("User not found");

			return ResultCommand.Ok<User, UserSelfViewModel>(user);
		}
	}
}
