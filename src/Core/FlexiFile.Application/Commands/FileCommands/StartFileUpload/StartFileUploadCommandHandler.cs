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
		private readonly IValidateUserStorageService _validateUserStorageService;

		public StartFileUploadHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService, IValidateUserStorageService validateUserStorageService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
			_validateUserStorageService = validateUserStorageService;
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

			var user = await _unitOfWork.UserRepository.GetByIdAsync(_userClaimsService.Id) ?? throw new Exception("User not found in database");
			if (!_validateUserStorageService.ValidateStorageForNewFile(user, request.FileSize)) {
				return ResultCommand.Unauthorized("Storage limit exceeded.", "storageLimitExceeded");
			}

			var file = new File {
				Id = Guid.NewGuid(),
				TypeId = type.Id,
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
