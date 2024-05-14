using FlexiFile.Application.Results;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Infrastructure.Services;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.CreateFirstSetup {
	public class CreateFirstSetupCommandHandler : IRequestHandler<CreateFirstSetupCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public CreateFirstSetupCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<IResultCommand> Handle(CreateFirstSetupCommand request, CancellationToken cancellationToken) {

			if (await _unitOfWork.UserRepository.AnyAsync()) {
				return ResultCommand.Unauthorized("There is already one user registered.", "notFirstAccess");
			}

			var user = new User {
				Id = Guid.NewGuid(),
				Name = request.Name,
				Email = request.Email,
				Password = PasswordService.HashPassword(request.Password),
				AccessLevel = AccessLevel.Root,
				Approved = true,
				ApprovedAt = DateTime.UtcNow,
				StorageUsed = 0,
				CreationDate = DateTime.UtcNow,
				LastUpdateDate = DateTime.UtcNow,
			};

			_unitOfWork.UserRepository.Add(user);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
