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
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using FlexiFile.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace FlexiFile.Application.Commands.ConvertCommands.StartConvertCommand {
	public class StartConvertCommandHandler : IRequestHandler<StartConvertCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IServiceProvider _serviceProvider;

		public StartConvertCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) {
			_unitOfWork = unitOfWork;
			_serviceProvider = serviceProvider;
		}

		public async Task Handle(StartConvertCommand request, CancellationToken cancellationToken) {
			var conversion = await _unitOfWork.FileConversionRepository.GetByIdAsync(request.ConversionId) ?? throw new Exception($"File conversion with id {request.ConversionId} not found");

			if (conversion.Status != ConvertStatus.AwaitingQueue) {
				throw new Exception($"File conversion with id {request.ConversionId} is not awaiting queue");
			}

			conversion.Status = ConvertStatus.AwaitingQueue;
			conversion.LastUpdateDate = DateTime.UtcNow;
			//await _unitOfWork.Commit();

			string interfaceName = conversion.FileTypeConversion.HandlerClassName;

			var assembly = Assembly.GetAssembly(typeof(IConvertFileService)) ?? throw new Exception("Assembly not found");
			var type = assembly.GetTypes().FirstOrDefault(t => t.Name == interfaceName) ?? throw new Exception("Type not found");

			var service = _serviceProvider.GetService(type);
			if (service is not IConvertFileService convertFileService) {
				throw new Exception("Service not found");
			}

			string directoryPath = $"/files/{conversion.File.OwnedByUserId}/{conversion.File.Id}";

			_ = convertFileService.ConvertFile(directoryPath, conversion.FileTypeConversion.FromType, conversion.FileTypeConversion.ToType);
		}
	}
}
