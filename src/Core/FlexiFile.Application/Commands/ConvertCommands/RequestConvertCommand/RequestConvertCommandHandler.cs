using FlexiFile.Application.HubClients;
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
using FlexiFile.Core.Models.Hubs.ConvertHub;
using FlexiFile.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand {
	public class RequestConvertCommandHandler : IRequestHandler<RequestConvertCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;
		private readonly ConvertHubClient _convertHubClient;

		public RequestConvertCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService, ConvertHubClient convertHubClient) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
			_convertHubClient = convertHubClient;
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

			Guid conversionId = Guid.NewGuid();
			var fileConversionRequest = new FileConversion {
				Id = conversionId,
				File = file,
				FileTypeConversion = fileConversion,
				Status = ConvertStatus.AwaitingQueue,
				PercentageComplete = 0,
				CreationDate = DateTime.UtcNow,
				LastUpdateDate = DateTime.UtcNow,
				ExtraInfo = null
			};

			_unitOfWork.FileConversionRepository.Add(fileConversionRequest);
			await _unitOfWork.Commit();

			await _convertHubClient.SendFileConvertRequestedAsync(new FileConvertRequestedInfo {
				ConversionId = conversionId
			});

			return ResultCommand.Created<FileConversion, FileConversionViewModel>(fileConversionRequest);
		}
	}
}
