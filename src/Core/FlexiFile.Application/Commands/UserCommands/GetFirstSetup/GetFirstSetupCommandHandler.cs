using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Infrastructure.Services;
using MediatR;

namespace FlexiFile.Application.Commands.UserCommands.GetFirstSetup;
public class GetFirstSetupCommandHandler : IRequestHandler<GetFirstSetupCommand, IResultCommand> {
	private readonly IUnitOfWork _unitOfWork;

	public GetFirstSetupCommandHandler(IUnitOfWork unitOfWork) {
		_unitOfWork = unitOfWork;
	}

	public async Task<IResultCommand> Handle(GetFirstSetupCommand request, CancellationToken cancellationToken) {

		if (await _unitOfWork.UserRepository.AnyAsync()) {
			return ResultCommand.Ok<GetFirstSetupViewModel, GetFirstSetupViewModel>(new GetFirstSetupViewModel(false));
		}

		return ResultCommand.Ok<GetFirstSetupViewModel, GetFirstSetupViewModel>(new GetFirstSetupViewModel(true));
	}
}
