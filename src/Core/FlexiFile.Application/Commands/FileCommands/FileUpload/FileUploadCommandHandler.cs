﻿using FlexiFile.Application.Results;
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

namespace FlexiFile.Application.Commands.FileCommands.FileUpload {
	public class FileUploadCommandHandler : IRequestHandler<FileUploadCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;
		private readonly IFileService _fileService;
		private readonly IValidateUserStorageService _validateUserStorageService;

		public FileUploadCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService, IFileService fileService, IValidateUserStorageService validateUserStorageService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
			_fileService = fileService;
			_validateUserStorageService = validateUserStorageService;
		}

		public async Task<IResultCommand> Handle(FileUploadCommand request, CancellationToken cancellationToken) {
			var file = await _unitOfWork.FileRepository.GetUserFileByIdAsync(request.FileId, _userClaimsService.Id);
			if (file is null) {
				return ResultCommand.BadRequest("File not found", "fileNotFound");
			}

			if (file.FinishedUpload) {
				return ResultCommand.BadRequest("File already uploaded", "fileAlreadyUploaded");
			}

			if (file.Size != request.File.Length) {
				return ResultCommand.BadRequest("File size does not match", "fileSizeDoesNotMatch");
			}

			var globalMaxFileSize = await _unitOfWork.SettingRepository.GetGlobalMaximumFileSize();
			if (globalMaxFileSize != 0 && file.Size > globalMaxFileSize) {
				return ResultCommand.Unauthorized("Size exceeds maximum limit.", "fileExceedsMaximumSize");
			}

			if (file.Type.MimeType != request.File.ContentType) {
				return ResultCommand.BadRequest("File type does not match", "fileTypeDoesNotMatch");
			}

			var user = file.OwnedByUser;
			if (!_validateUserStorageService.ValidateStorageForNewFile(user, file.Size)) {
				return ResultCommand.Unauthorized("Storage limit exceeded.", "storageLimitExceeded");
			}

			user.StorageUsed += file.Size;
			file.FinishedUpload = true;
			file.FinishedUploadAt = DateTime.UtcNow;

			await _fileService.UploadFile(request.File, file.Id, _userClaimsService.Id);

			await _unitOfWork.Commit();

			return ResultCommand.Ok<File, FileViewModel>(file);
		}
	}
}
