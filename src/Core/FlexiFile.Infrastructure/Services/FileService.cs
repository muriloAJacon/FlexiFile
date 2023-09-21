using FlexiFile.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Infrastructure.Services {
	public class FileService : IFileService {
		private ILogger<FileService> _logger;

		public FileService(ILogger<FileService> logger) {
			_logger = logger;
		}

		public async Task UploadFile(IFormFile file, Guid fileId, Guid userId) {
			var dir = new DirectoryInfo($"./files/{userId}/{fileId}");
			if (!dir.Exists) {
				_logger.LogInformation("Creating directory {path}", dir.FullName);
				dir.Create();
			}

			using var stream = new FileStream(Path.Combine(dir.FullName, "original"), FileMode.CreateNew, FileAccess.Write);
			await file.CopyToAsync(stream);

			await stream.FlushAsync();
			await stream.DisposeAsync();
		}
	}
}
