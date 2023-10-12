using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using ImageMagick;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;
using static FlexiFile.Core.Interfaces.Services.ConvertServices.IConvertFileService;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class ConvertImageService : IConvertImageService {
		private readonly ILogger<ConvertImageService> _logger;

		public ConvertImageService(ILogger<ConvertImageService> logger) {
			_logger = logger;
		}

		public async Task ConvertFile(ChannelWriter<ConvertProgressNotificationEvent> notificationChannelWriter, Core.Entities.Postgres.File file, string fileDirectory, FileType inputFileType, FileType outputFileType) {
			try {
				var directory = new DirectoryInfo(fileDirectory);

				string inputFilePath = Path.Combine(directory.FullName, "original");

				Guid outputFileId = Guid.NewGuid();
				string outputPath = Path.Combine(directory.FullName, outputFileId.ToString());

				using var image = new MagickImage(inputFilePath);

				MagickFormat format = outputFileType.MimeTypes.First() switch {
					"image/jpeg" => MagickFormat.Jpeg,
					"image/png" => MagickFormat.Png,
					"image/gif" => MagickFormat.Gif,
					"image/tiff" => MagickFormat.Tiff,
					"image/vnd.microsoft.icon" => MagickFormat.Ico,
					"image/svg+xml" => MagickFormat.Svg,
					_ => throw new Exception("Mime type not supported")
				};

				if (format == MagickFormat.Ico) {
					image.Resize(16, 16); // TODO: IMPLEMENT BETTER
				}

				await image.WriteAsync(outputPath, format);

				var @event = new ConvertProgressNotificationEvent {
					ConvertStatus = Core.Enums.ConvertStatus.Completed,
					PercentageComplete = 100
				};

				await notificationChannelWriter.WriteAsync(@event);
			} catch (Exception ex) {
				_logger.LogError(ex, "Failed to convert file");

				var @event = new ConvertProgressNotificationEvent {
					ConvertStatus = Core.Enums.ConvertStatus.Failed,
				};

				await notificationChannelWriter.WriteAsync(@event);
			}
		}
	}
}
