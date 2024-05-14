using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using FlexiFile.Core.Models.ConversionParameters.RearrangeDocument;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class ITextRearrangeDocumentService : IRearrangeDocumentService {
		private readonly ILogger<ITextRearrangeDocumentService> _logger;

		public ITextRearrangeDocumentService(ILogger<ITextRearrangeDocumentService> logger) {
			_logger = logger;
		}

		public async Task ConvertFile(ChannelWriter<EventArgs> notificationChannelWriter, FileConversion conversion, string fileDirectory, FileType inputFileType, FileType? outputFileType) {
			try {
				if (conversion.FileConversionOrigins.Count > 1) {
					throw new ArgumentException("This conversion type does not support multiple origin files", nameof(conversion));
				}

				if (conversion.ExtraInfo is null) {
					throw new ArgumentException("This conversion type requires extra info", nameof(conversion));
				}

				var directory = new DirectoryInfo(fileDirectory);

				var inputFile = conversion.FileConversionOrigins.First();
				string inputFilePath = Path.Combine(directory.FullName, inputFile.FileId.ToString());

				string outputPath = Path.Combine(directory.FullName, conversion.Id.ToString());

				using PdfReader reader = new(inputFilePath);
				using PdfDocument readingDocument = new(reader);

				RearrangeDocumentParameters parameters = conversion.ExtraInfo.Value.Deserialize<RearrangeDocumentParameters>() ?? throw new Exception("Unable to deserialize parameters");

				Guid outputFileId = Guid.NewGuid();

				string outputFilePath = Path.Combine(outputPath, outputFileId.ToString());
				using PdfWriter writer = new(outputFilePath);
				using PdfDocument writingDocument = new(writer);

				foreach (int originalPageNumber in parameters.OriginalPageNumbers) {
					readingDocument.CopyPagesTo(originalPageNumber, originalPageNumber, writingDocument);
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
