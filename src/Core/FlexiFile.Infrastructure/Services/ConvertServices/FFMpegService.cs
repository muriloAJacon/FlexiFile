﻿using FFMpegCore;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using File = System.IO.File;
using Microsoft.Extensions.Logging;
using FlexiFile.Core.Events;
using System.Threading.Channels;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class FFMpegService : IConvertVideoService, IConvertAudioService {
		private readonly ILogger<FFMpegService> _logger;

		public FFMpegService(ILogger<FFMpegService> logger) {
			_logger = logger;
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

				string tempOutputPath = outputPath + "." + outputFileType.MimeTypes.First() switch {
					"video/mp4" => "mp4",
					"video/mpeg" => "mpeg",
					"video/webm" => "webm",
					"video/x-matroska" => "mkv",
					"audio/mp4" => "m4a",
					"audio/mpeg" => "mp3",
					_ => throw new Exception("Mime type not supported")
				};

				var mediaInfo = await FFProbe.AnalyseAsync(inputFilePath);

				var result = await FFMpegArguments.FromFileInput(inputFilePath).OutputToFile(tempOutputPath, overwrite: true, delegate (FFMpegArgumentOptions options) {
					options.UsingMultithreading(true);
				}).NotifyOnProgress((progress) => HandleProgress(progress, conversion, notificationChannelWriter), mediaInfo.Duration).ProcessAsynchronously();

				if (!result) {
					throw new Exception("Failed to convert file");
				}

				File.Move(tempOutputPath, outputPath);

				var fileResultEvent = new ConvertFileResultEvent {
					EventId = Guid.NewGuid(),
					FileId = outputFileId,
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
					EventId = Guid.NewGuid(),
					ConvertStatus = Core.Enums.ConvertStatus.Failed,
				};

				await notificationChannelWriter.WriteAsync(@event);
			}
		}

		private async void HandleProgress(double progress, FileConversion conversion, ChannelWriter<EventArgs> channelWriter) {
			_logger.LogInformation("Conversion {}: progress: {}", conversion.Id, progress);

			var @event = new ConvertProgressNotificationEvent {
				EventId = Guid.NewGuid(),
				ConvertStatus = Core.Enums.ConvertStatus.InProgress,
				PercentageComplete = progress
			};

			await channelWriter.WriteAsync(@event);
		}
	}
}
