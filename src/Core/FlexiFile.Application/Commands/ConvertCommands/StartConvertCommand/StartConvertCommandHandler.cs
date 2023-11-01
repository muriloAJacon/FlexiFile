using FlexiFile.Application.Commands.FileCommands.CreateFileConvertResult;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Enums;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading.Channels;

namespace FlexiFile.Application.Commands.ConvertCommands.StartConvertCommand {
	public class StartConvertCommandHandler : IRequestHandler<StartConvertCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IServiceProvider _serviceProvider;
		private readonly ILogger<StartConvertCommandHandler> _logger;
		private readonly IMediator _mediator;

		public StartConvertCommandHandler(IUnitOfWork unitOfWork, IServiceProvider serviceProvider, ILogger<StartConvertCommandHandler> logger, IMediator mediator) {
			_unitOfWork = unitOfWork;
			_serviceProvider = serviceProvider;
			_logger = logger;
			_mediator = mediator;
		}

		public async Task Handle(StartConvertCommand request, CancellationToken cancellationToken) {
			var conversion = await _unitOfWork.FileConversionRepository.GetByIdAsync(request.ConversionId) ?? throw new Exception($"File conversion with id {request.ConversionId} not found");

			if (conversion.Status != ConvertStatus.AwaitingQueue) {
				throw new Exception($"File conversion with id {request.ConversionId} is not awaiting queue");
			}

			await _mediator.Send(new UpdateConvertProgressCommand.UpdateConvertProgressCommand {
				ConversionId = conversion.Id,
				ConvertStatus = ConvertStatus.InQueue
			}, cancellationToken);

			string interfaceName = conversion.FileTypeConversion.HandlerClassName;

			var assembly = Assembly.GetAssembly(typeof(IConvertFileService)) ?? throw new Exception("Assembly not found");
			var type = assembly.GetTypes().FirstOrDefault(t => t.Name == interfaceName) ?? throw new Exception("Type not found");

			var service = _serviceProvider.GetRequiredService(type);
			if (service is not IConvertFileService convertFileService) {
				throw new TargetException($"Service is not an instance of {nameof(IConvertFileService)}");
			}

			string userDirectoryPath = $"/files/{conversion.UserId}";

			string conversionDirectoryPath = Path.Combine(userDirectoryPath, conversion.Id.ToString());
			var directoryInfo = new DirectoryInfo(conversionDirectoryPath);
			if (!directoryInfo.Exists) {
				_logger.LogInformation("Creating path {} for conversion", directoryInfo.FullName);
				directoryInfo.Create();
			}

			var channel = Channel.CreateUnbounded<EventArgs>();

			_logger.LogDebug("Starting conversion task");

			var convertTask = convertFileService.ConvertFile(channel, conversion, userDirectoryPath, conversion.FileTypeConversion.FromType, conversion.FileTypeConversion.ToType);

			_ = convertTask.ContinueWith(_ => {
				_logger.LogDebug("Conversion done - Closing channel");
				channel.Writer.Complete();
			}, cancellationToken);

			while (await channel.Reader.WaitToReadAsync(cancellationToken)) {
				var @event = await channel.Reader.ReadAsync(cancellationToken);

				switch (@event) {
					case ConvertProgressNotificationEvent progressNotificationEvent:
						await HandleConvertProgressNotificationEvent(conversion, progressNotificationEvent);
						break;
					case ConvertFileResultEvent convertFileResultEvent:
						await HandleConvertFileResultEvent(conversion, convertFileResultEvent);
						break;
				}
			}

			_logger.LogDebug("Done");
		}

		private async Task HandleConvertProgressNotificationEvent(FileConversion conversion, ConvertProgressNotificationEvent @event) {
			_logger.LogDebug("Reading event {} - status {} percentage {}", @event.EventId, @event.ConvertStatus, @event.PercentageComplete);

			await _mediator.Send(new UpdateConvertProgressCommand.UpdateConvertProgressCommand {
				ConversionId = conversion.Id,
				ConvertStatus = @event.ConvertStatus,
				PercentageComplete = @event.PercentageComplete
			});

			_logger.LogDebug("Successfully processed event {}", @event.EventId);
		}

		private async Task HandleConvertFileResultEvent(FileConversion conversion, ConvertFileResultEvent @event) {
			_logger.LogDebug("Reading event {} - file id {} order {}", @event.EventId, @event.FileId, @event.Order);

			await _mediator.Send(new CreateFileConvertResultCommand {
				FileId = @event.FileId,
				ConversionId = conversion.Id,
				Order = @event.Order
			});

			_logger.LogDebug("Successfully processed event {}", @event.EventId);
		}
	}
}
