﻿using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.Threading.Channels;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class MagickService : IConvertImageService {
		private readonly ILogger<MagickService> _logger;
		private readonly IValidateUserStorageService _validateUserStorageService;

		public MagickService(ILogger<MagickService> logger, IValidateUserStorageService validateUserStorageService) {
			_logger = logger;
			_validateUserStorageService = validateUserStorageService;
		}

		public async Task ConvertFile(ChannelWriter<EventArgs> notificationChannelWriter, FileConversion conversion, string fileDirectory, FileType inputFileType, FileType? outputFileType) {
			try {
				if (outputFileType is null) {
					throw new ArgumentNullException(nameof(outputFileType), "Output file type cannot be null for this conversion type");
				}

				if (conversion.FileConversionOrigins.Count > 1) {
					throw new ArgumentException("This conversion type does not support multiple origin files", nameof(conversion));
				}

				var directory = new DirectoryInfo(fileDirectory);

				var inputFile = conversion.FileConversionOrigins.First();
				string inputFilePath = Path.Combine(directory.FullName, inputFile.FileId.ToString());

				Guid outputFileId = Guid.NewGuid();
				string outputPath = Path.Combine(directory.FullName, conversion.Id.ToString(), outputFileId.ToString());

				using var image = new MagickImage(inputFilePath);

				MagickFormat format = outputFileType.MimeType switch {
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

				long fileSize = new FileInfo(outputPath).Length;
				if (!_validateUserStorageService.ValidateStorageForConvertedFile(conversion.User, fileSize)) {
					System.IO.File.Delete(outputPath);
					throw new Exception("User storage exceeded");
				}

				var fileResultEvent = new ConvertFileResultEvent {
					TypeId = outputFileType.Id,
					EventId = Guid.NewGuid(),
					FileId = outputFileId,
					Size = fileSize,
					Order = 1
				};
				await notificationChannelWriter.WriteAsync(fileResultEvent);

				var @event = new ConvertProgressNotificationEvent {
					EventId = Guid.NewGuid(),
					ConvertStatus = Core.Enums.ConvertStatus.Completed,
					PercentageComplete = 100
				};

				await notificationChannelWriter.WriteAsync(@event);
			} catch (Exception ex) {
				_logger.LogError(ex, "Failed to convert file");

				var @event = new ConvertProgressNotificationEvent {
					EventId= Guid.NewGuid(),
					ConvertStatus = Core.Enums.ConvertStatus.Failed,
				};

				await notificationChannelWriter.WriteAsync(@event);
			}
		}
	}
}
