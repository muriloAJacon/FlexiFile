using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.File;
using System.Threading.Channels;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class ITextSplitDocumentService : ISplitDocumentService {
		private readonly ILogger<ITextSplitDocumentService> _logger;
		private readonly IValidateUserStorageService _validateUserStorageService;

		public ITextSplitDocumentService(ILogger<ITextSplitDocumentService> logger, IValidateUserStorageService validateUserStorageService) {
			_logger = logger;
			_validateUserStorageService = validateUserStorageService;
		}

		public async Task ConvertFile(ChannelWriter<EventArgs> notificationChannelWriter, FileConversion conversion, string fileDirectory, FileType inputFileType, FileType? outputFileType) {
			try {
				if (conversion.FileConversionOrigins.Count > 1) {
					throw new ArgumentException("This conversion type does not support multiple origin files", nameof(conversion));
				}

				var directory = new DirectoryInfo(fileDirectory);

				var inputFile = conversion.FileConversionOrigins.First();
				string inputFilePath = Path.Combine(directory.FullName, inputFile.FileId.ToString());

				string outputPath = Path.Combine(directory.FullName, conversion.Id.ToString());

				using PdfReader reader = new(inputFilePath);
				using PdfDocument separatingPdf = new(reader);

				int pages = separatingPdf.GetNumberOfPages();

				for (int i = 1; i <= pages; i++) {
					Guid outputFileId = Guid.NewGuid();

					string outputFilePath = Path.Combine(outputPath, outputFileId.ToString());
					using PdfWriter writer = new(outputFilePath);
					using PdfDocument pdf = new(writer);
					separatingPdf.CopyPagesTo(i, i, pdf);

					pdf.Close();

					long fileSize = new FileInfo(outputFilePath).Length;
					if (!_validateUserStorageService.ValidateStorageForConvertedFile(conversion.User, fileSize)) {
						System.IO.File.Delete(outputFilePath);
						throw new Exception("User storage exceeded");
					}

					var fileResultEvent = new ConvertFileResultEvent {
						EventId = Guid.NewGuid(),
						FileId = outputFileId,
						Size = fileSize,
						Order = i
					};
					await notificationChannelWriter.WriteAsync(fileResultEvent);

					var progressEvent = new ConvertProgressNotificationEvent {
						EventId = Guid.NewGuid(),
						ConvertStatus = Core.Enums.ConvertStatus.InProgress,
						PercentageComplete = (double)i / pages * 100
					};
					await notificationChannelWriter.WriteAsync(progressEvent);
				}

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
