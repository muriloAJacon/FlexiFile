using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.UserViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.UserCommands.GetUsers {
	public class GetUsersCommandHandler : IRequestHandler<GetUsersCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetUsersCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<IResultCommand> Handle(GetUsersCommand request, CancellationToken cancellationToken) {

			var users = await _unitOfWork.UserRepository.GetAllAsync();

			return ResultCommand.Ok<List<User>, List<UserViewModel>>(users.ToList());
		}
	}
}
