using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.FileViewModels;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.FileCommands.GetFiles {
	public class GetFilesCommandHandler : IRequestHandler<GetFilesCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public GetFilesCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(GetFilesCommand request, CancellationToken cancellationToken) {
			var files = await _unitOfWork.FileRepository.GetUploadedUserFilesWithExceptionsAsync(request.IgnoreFileIds, _userClaimsService.Id);

			return ResultCommand.Ok<List<Core.Entities.Postgres.File>, List<FileViewModel>>(files);
		}
	}
}
