using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlexiFile.API.Controllers {
	[Route("files")]
	[ApiController]
	public class FileViewController : ControllerBase {

		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _userClaimsService;

		public FileViewController(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_userClaimsService = userClaimsService;
		}

		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetBaseFile(Guid id, [FromQuery] bool download = false) {
			// TODO: CHECK IF USER HAS ACCESS TO FILE
			var fileInfo = await _unitOfWork.FileRepository.GetByIdAsync(id);
			if (fileInfo is null) {
				return NotFound();
			}

			string filePath = $"/files/{fileInfo.OwnedByUserId}/{fileInfo.Id}";

			string? fileName = download ? fileInfo.OriginalName : null;

			return PhysicalFile(filePath, fileInfo.Type.MimeTypes.First(), fileName);
		}

		[HttpGet("{conversionId:guid}/{fileId:guid}")]
		public async Task<IActionResult> GetConvertedFile(Guid conversionId, Guid fileId, [FromQuery] bool download = false) {
			// TODO: CHECK IF USER HAS ACCESS TO FILE
			var conversionInfo = await _unitOfWork.FileConversionRepository.GetByIdAsync(conversionId);
			if (conversionInfo is null) {
				return NotFound();
			}

			var file = conversionInfo.FileConversionResults.SingleOrDefault(x => x.Id == fileId);
			if (file is null) {
				return NotFound();
			}

			string filePath = $"/files/{conversionInfo.FileConversionOrigins.First().File.OwnedByUserId}/{conversionInfo.Id}/{file.Id}";

			string? fileName = download ? file.Id.ToString() : null; // TODO: ADD EXTENSION

			string mimeType = conversionInfo.FileTypeConversion.ToType?.MimeTypes.First() ?? "application/pdf"; // TODO: GET FROM FILE

			return PhysicalFile(filePath, mimeType, fileName);
		}
	}
}
