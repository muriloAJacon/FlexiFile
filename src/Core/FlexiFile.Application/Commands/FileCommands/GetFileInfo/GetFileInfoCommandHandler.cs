using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.FileViewModels;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using MediatR;

namespace FlexiFile.Application.Commands.FileCommands.GetFileInfo {
	public class GetFileInfoCommandHandler : IRequestHandler<GetFileInfoCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public GetFileInfoCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(GetFileInfoCommand request, CancellationToken cancellationToken) {
			var file = await _unitOfWork.FileRepository.GetUserFileByIdAsync(request.Id, _userClaimsService.Id);
			if (file == null) {
				return ResultCommand.NotFound("The specified file was not found.", "fileNotFound");
			}

			return ResultCommand.Ok<Core.Entities.Postgres.File, FileViewModel>(file);
		}
	}
}
