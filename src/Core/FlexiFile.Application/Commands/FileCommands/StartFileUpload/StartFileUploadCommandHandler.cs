using FlexiFile.Application.Results;
using FlexiFile.Application.Security;
using FlexiFile.Application.ViewModels;
using FlexiFile.Application.ViewModels.FileViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Application.Commands.FileCommands.StartFileUpload {
	public class StartFileUploadHandler : IRequestHandler<StartFileUploadCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public StartFileUploadHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(StartFileUploadCommand request, CancellationToken cancellationToken) {

			var type = await _unitOfWork.FileTypeRepository.GetByMimeTypeAsync(request.MimeType);
			if (type is null) {
				return ResultCommand.Unauthorized("Unsupported file type.", "unsupportedFileType");
			}

			var globalMaxFileSize = await _unitOfWork.SettingRepository.GetGlobalMaximumFileSize();
			if (globalMaxFileSize != 0 && request.FileSize > globalMaxFileSize) {
				return ResultCommand.Unauthorized("Size exceeds maximum limit.", "fileExceedsMaximumSize");
			}

			// TODO: Validate account size limit

			var file = new File {
				Id = Guid.NewGuid(),
				Type = type,
				OwnedByUserId = _userClaimsService.Id,
				Size = request.FileSize,
				OriginalName = request.FileName,
				SubmittedAt = DateTime.UtcNow,
				FinishedUpload = false
			};

			_unitOfWork.FileRepository.Add(file);
			await _unitOfWork.Commit();

			return ResultCommand.Created<File, FileViewModel>(file);
		}
	}
}
