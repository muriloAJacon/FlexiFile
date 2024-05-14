using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class ITextMergeDocumentService : IMergeDocumentService {
		private readonly ILogger<ITextMergeDocumentService> _logger;

		public ITextMergeDocumentService(ILogger<ITextMergeDocumentService> logger) {
			_logger = logger;
		}

		public async Task ConvertFile(ChannelWriter<EventArgs> notificationChannelWriter, FileConversion conversion, string fileDirectory, FileType inputFileType, FileType? outputFileType) {
			try {
				if (conversion.FileConversionOrigins.Count == 1) {
					throw new ArgumentException("This conversion type requires at least two origin files", nameof(conversion));
				}

				var directory = new DirectoryInfo(fileDirectory);

				string outputPath = Path.Combine(directory.FullName, conversion.Id.ToString());

				Guid outputFileId = Guid.NewGuid();
				string outputFilePath = Path.Combine(outputPath, outputFileId.ToString());

				using PdfWriter writer = new(outputFilePath);
				using PdfDocument pdf = new(writer);

				var inputFiles = conversion.FileConversionOrigins.ToList();
				for (int i = 0; i < inputFiles.Count; i++) { 
					var inputFile = inputFiles[i];

					string inputFilePath = Path.Combine(directory.FullName, inputFile.FileId.ToString());
					using PdfReader reader = new(inputFilePath);
					using PdfDocument mergingPdf = new(reader);

					mergingPdf.CopyPagesTo(1, mergingPdf.GetNumberOfPages(), pdf);

					var progressEvent = new ConvertProgressNotificationEvent {
						EventId = Guid.NewGuid(),
						ConvertStatus = Core.Enums.ConvertStatus.InProgress,
						PercentageComplete = (double)i / conversion.FileConversionOrigins.Count * 100
					};
					await notificationChannelWriter.WriteAsync(progressEvent);
				}

				var fileResultEvent = new ConvertFileResultEvent {
					EventId = Guid.NewGuid(),
					FileId = outputFileId,
					Size = new FileInfo(outputFilePath).Length,
					Order = 1
				};
				await notificationChannelWriter.WriteAsync(fileResultEvent);

				var completedEvent = new ConvertProgressNotificationEvent {
					EventId = Guid.NewGuid(),
					ConvertStatus = Core.Enums.ConvertStatus.Completed,
					PercentageComplete = 100
				};
				await notificationChannelWriter.WriteAsync(completedEvent);
			} catch (Exception ex) {
				_logger.LogError(ex, "Failed to convert file");
				var @event = new ConvertProgressNotificationEvent {
					EventId = Guid.NewGuid(),
					ConvertStatus = Core.Enums.ConvertStatus.Failed,
				};

				await notificationChannelWriter.WriteAsync(@event);
			}
		}
	}
}
