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

			var files = await _unitOfWork.FileRepository.GetUserFilesByIdAsync(request.FileIds, _userClaimsService.Id);
			var foundFilesIds = files.Select(x => x.Id);
			var notFoundFilesIds = request.FileIds.Except(foundFilesIds);

			if (notFoundFilesIds.Any()) {
				return ResultCommand.BadRequest($"Files: {string.Join(',', notFoundFilesIds)} not found", "fileNotFound");
			}

			var fileConversion = await _unitOfWork.FileTypeConversionRepository.GetByIdAsync(request.ConversionId);
			if (fileConversion is null) {
				return ResultCommand.BadRequest("File conversion not found", "fileConversionNotFound");
			}

			Guid conversionId = Guid.NewGuid();
			var fileConversionRequest = new FileConversion {
				Id = conversionId,
				FileConversionOrigins = new List<FileConversionOrigin>(),
				FileTypeConversion = fileConversion,
				Status = ConvertStatus.AwaitingQueue,
				PercentageComplete = 0,
				CreationDate = DateTime.UtcNow,
				LastUpdateDate = DateTime.UtcNow,
				ExtraInfo = null
			};

			for (int i = 0; i < request.FileIds.Count; i++) {
				Guid fileId = request.FileIds[i];
				File file = files.Single(x => x.Id == fileId);
				var fileConversionOrigin = new FileConversionOrigin {
					Id = Guid.NewGuid(),
					File = file,
					Order = i + 1,
					ExtraInfo = null
				};

				fileConversionRequest.FileConversionOrigins.Add(fileConversionOrigin);
			}

			_unitOfWork.FileConversionRepository.Add(fileConversionRequest);
			await _unitOfWork.Commit();

			_ = _convertHubClient.SendFileConvertRequestedAsync(new FileConvertRequestedInfo {
				ConversionId = conversionId
			}).ConfigureAwait(false);

			return ResultCommand.Created<FileConversion, FileConversionViewModel>(fileConversionRequest);
		}
	}
}
