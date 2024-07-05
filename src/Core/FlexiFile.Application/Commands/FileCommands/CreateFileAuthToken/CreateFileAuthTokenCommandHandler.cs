using FlexiFile.Application.Results;
using FlexiFile.Application.Security.FileAccess;
using FlexiFile.Application.ViewModels;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.FileCommands.CreateFileAuthToken {
	public class CreateFileAuthTokenCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService, FileSigningConfigurations fileSigningConfigurations, FileTokenConfigurations fileTokenConfigurations) : IRequestHandler<CreateFileAuthTokenCommand, IResultCommand> {

		private readonly IUnitOfWork _unitOfWork = unitOfWork;
		private readonly IUserClaimsService _userClaimsService = userClaimsService;
		private readonly FileSigningConfigurations _fileSigningConfigurations = fileSigningConfigurations;
		private readonly FileTokenConfigurations _fileTokenConfigurations = fileTokenConfigurations;

		public async Task<IResultCommand> Handle(CreateFileAuthTokenCommand request, CancellationToken cancellationToken) {
			string filePath;
			switch (request.FileType) {
				case FileType.OriginalFile:
					var file = await _unitOfWork.FileRepository.GetUserFileByIdAsync(request.FileId, _userClaimsService.Id);
					if (file is null) {
						return ResultCommand.BadRequest("File not found", "fileNotFound");
					}
					filePath = $"/files/{file.OwnedByUserId}/{file.Id}";
					break;
				case FileType.ConvertedFile:
					var convertedFile = await _unitOfWork.FileConversionResultRepository.GetUserFileByIdAsync(request.FileId, _userClaimsService.Id);
					if (convertedFile is null) {
						return ResultCommand.BadRequest("File not found", "fileNotFound");
					}
					filePath = $"/files/{convertedFile.FileConversion.UserId}/{convertedFile.FileConversion.Id}/{convertedFile.Id}";
					break;
				default:
					throw new NotSupportedException();
			}

			var token = FileTokenService.GenerateToken(request.FileId, filePath, request.FileType, _fileSigningConfigurations, _fileTokenConfigurations);

			return ResultCommand.Ok<FileBearerTokenViewModel, FileBearerTokenViewModel>(token);
		}
	}
}
