using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using ImageMagick;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class MagickService : IConvertImageService {
		private readonly ILogger<MagickService> _logger;

		public MagickService(ILogger<MagickService> logger) {
			_logger = logger;
		}

		public async Task ConvertFile(ChannelWriter<EventArgs> notificationChannelWriter, Core.Entities.Postgres.File file, string fileDirectory, FileType inputFileType, FileType outputFileType) {
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

				var fileResultEvent = new ConvertFileResultEvent {
					EventId = Guid.NewGuid(),
					FileId = outputFileId,
					Order = 1
				};
				await notificationChannelWriter.WriteAsync(fileResultEvent);

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
