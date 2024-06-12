using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using FlexiFile.Core.Models.ConversionParameters.RearrangeDocument;
using iText.Kernel.Pdf;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.File;
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
		private readonly IValidateUserStorageService _validateUserStorageService;
		private readonly IUnitOfWork _unitOfWork;

		public ITextRearrangeDocumentService(ILogger<ITextRearrangeDocumentService> logger, IValidateUserStorageService validateUserStorageService, IUnitOfWork unitOfWork) {
			_logger = logger;
			_validateUserStorageService = validateUserStorageService;
			_unitOfWork = unitOfWork;
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

				writingDocument.Close();

				long fileSize = new FileInfo(outputFilePath).Length;

				if (!_validateUserStorageService.ValidateStorageForConvertedFile(conversion.User, fileSize)) {
					System.IO.File.Delete(outputFilePath);
					throw new Exception("User storage exceeded");
				}

				var pdfFileType = await _unitOfWork.FileTypeRepository.GetByMimeTypeAsync("application/pdf") ?? throw new Exception("PDF File Type not found");
				var fileResultEvent = new ConvertFileResultEvent {
					TypeId = pdfFileType.Id,
					EventId = Guid.NewGuid(),
					FileId = outputFileId,
					Size = fileSize,
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
