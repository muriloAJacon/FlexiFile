using FlexiFile.Application.HubClients;
using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.FileConversionViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using FlexiFile.Core.Models.Hubs.ConvertHub;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand {
	public class RequestConvertCommandHandler : IRequestHandler<RequestConvertCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;
		private readonly ConvertHubClient _convertHubClient;
		private readonly IServiceProvider _serviceProvider;

		public RequestConvertCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService, ConvertHubClient convertHubClient, IServiceProvider serviceProvider) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
			_convertHubClient = convertHubClient;
			_serviceProvider = serviceProvider;
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

			var wrongTypeFiles = files.Where(x => x.TypeId != fileConversion.FromTypeId).ToList();
			if (wrongTypeFiles.Any()) {
				return ResultCommand.BadRequest($"Files: {string.Join(',', wrongTypeFiles.Select(x => x.Id))} are not of type {fileConversion.FromType.Description}", "wrongFileType");
			}

			if (request.FileIds.Count < fileConversion.MinNumberFiles) {
				return ResultCommand.BadRequest($"Minimum number of files for this conversion type is {fileConversion.MinNumberFiles}", "minNumberFiles");
			}

			if (request.FileIds.Count > fileConversion.MaxNumberFiles) {
				return ResultCommand.BadRequest($"Maximum number of files for this conversion type is {fileConversion.MaxNumberFiles}", "maxNumberFiles");
			}

			JsonElement? extraParameters = null;
			if (fileConversion.ModelClassName is not null) {
				if (request.ExtraParameters is null) {
					return ResultCommand.BadRequest($"Extra parameters is required", "extraParametersRequired");
				}

				// TODO: Change get assembly to different type, IConvertFileService is not related to these models
				var assembly = Assembly.GetAssembly(typeof(IConvertFileService)) ?? throw new Exception("Assembly not found");
				var type = assembly.GetTypes().FirstOrDefault(t => t.Name == fileConversion.ModelClassName) ?? throw new Exception("Type not found");

				JsonSerializerOptions options = new() {
					PropertyNameCaseInsensitive = true
				};
				var parameters = request.ExtraParameters.Value.Deserialize(type, options);

				var validator = (IValidator)_serviceProvider.GetRequiredService(typeof(IValidator<>).MakeGenericType(type));
				var context = typeof(ValidationContext<>).MakeGenericType(type).GetConstructor(new Type[] { type }).Invoke(new object[] { parameters }) as IValidationContext;

				var validationResult = validator.Validate(context);

				if (!validationResult.IsValid) {
					return ResultCommand.BadRequest("TODO: RESULT", "TODO");
				}

				extraParameters = JsonSerializer.SerializeToElement(parameters);
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
				ExtraInfo = extraParameters,
				UserId = _userClaimsService.Id
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
			});

			return ResultCommand.Created<FileConversion, FileConversionViewModel>(fileConversionRequest);
		}
	}
}
