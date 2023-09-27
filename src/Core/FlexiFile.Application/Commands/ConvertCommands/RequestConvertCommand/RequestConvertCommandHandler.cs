using FlexiFile.Application.Results;
using FlexiFile.Application.Security;
using FlexiFile.Application.ViewModels;
using FlexiFile.Application.ViewModels.FileConversionViewModels;
using FlexiFile.Application.ViewModels.FileViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand {
	public class RequestConvertCommandHandler : IRequestHandler<RequestConvertCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public RequestConvertCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(RequestConvertCommand request, CancellationToken cancellationToken) {
			var file = await _unitOfWork.FileRepository.GetUserFileByIdAsync(request.FileId, _userClaimsService.Id);
			if (file is null) {
				return ResultCommand.BadRequest("File not found", "fileNotFound");
			}

			var fileConversion = await _unitOfWork.FileTypeConversionRepository.GetByFromFileTypeToFileTypeAsync(file.TypeId, request.ToTypeId);
			if (fileConversion is null) {
				return ResultCommand.BadRequest("File conversion not found", "fileConversionNotFound");
			}

			var fileConversionRequest = new FileConversion {
				Id = Guid.NewGuid(),
				File = file,
				FileTypeConversion = fileConversion,
				Status = ConvertStatus.AwaitingQueue,
				PercentageComplete = 0,
				CreationDate = DateTime.UtcNow,
				LastUpdateDate = DateTime.UtcNow,
				ExtraInfo = null
			};

			// TODO: SEND TO WORKER

			_unitOfWork.FileConversionRepository.Add(fileConversionRequest);
			await _unitOfWork.Commit();

			return ResultCommand.Created<FileConversion, FileConversionViewModel>(fileConversionRequest);
		}
	}
}
