using FFMpegCore;
using FFMpegCore.Arguments;
using FFMpegCore.Enums;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using System.Drawing;
using System;
using File = System.IO.File;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Forms;
using FlexiFile.Core.Events;
using static FlexiFile.Core.Interfaces.Services.ConvertServices.IConvertFileService;
using System.Diagnostics;
using System.Threading.Channels;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class FFMpegService : IConvertVideoService, IConvertAudioService {
		private readonly ILogger<FFMpegService> _logger;

		public FFMpegService(ILogger<FFMpegService> logger) {
			_logger = logger;
		}

		public async Task ConvertFile(ChannelWriter<ConvertProgressNotificationEvent> notificationChannelWriter, Core.Entities.Postgres.File file, string fileDirectory, FileType inputFileType, FileType outputFileType) {
			try {

				var directory = new DirectoryInfo(fileDirectory);

				string inputFilePath = Path.Combine(directory.FullName, "original");

				Guid outputFileId = Guid.NewGuid();
				string outputPath = Path.Combine(directory.FullName, outputFileId.ToString());

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
				}).NotifyOnProgress((progress) => HandleProgress(progress, file, notificationChannelWriter), mediaInfo.Duration).ProcessAsynchronously();

				if (!result) {
					throw new Exception("Failed to convert file");
				}

				File.Move(tempOutputPath, outputPath);

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

		private async void HandleProgress(double progress, Core.Entities.Postgres.File file, ChannelWriter<ConvertProgressNotificationEvent> channelWriter) {
			_logger.LogInformation("File {}: progress: {}", file.Id, progress);

			var @event = new ConvertProgressNotificationEvent {
				EventId = Guid.NewGuid(),
				ConvertStatus = Core.Enums.ConvertStatus.InProgress,
				PercentageComplete = progress
			};

			await channelWriter.WriteAsync(@event);
		}
	}
}
