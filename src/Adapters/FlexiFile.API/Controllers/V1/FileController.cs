using Asp.Versioning;
using FlexiFile.Application.Commands.ConvertCommands.GetAvailableConversionsCommand;
using FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand;
using FlexiFile.Application.Commands.FileCommands.CreateFileAuthToken;
using FlexiFile.Application.Commands.FileCommands.FileUpload;
using FlexiFile.Application.Commands.FileCommands.GetFileInfo;
using FlexiFile.Application.Commands.FileCommands.GetFiles;
using FlexiFile.Application.Commands.FileCommands.StartFileUpload;
using FlexiFile.Application.ViewModels.FileViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web;

namespace FlexiFile.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[Authorize]
	[ApiController]
	public class FileController : ControllerBase {
		private readonly IMediator _mediator;

		public FileController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpGet("{id:guid}")]
		[ProducesResponseType(typeof(FileViewModel), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> GetFile(Guid id) => await _mediator.Send(new GetFileInfoCommand(id));

		[HttpGet]
		[ProducesResponseType(typeof(List<FileViewModel>), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> GetFiles([FromQuery] string? ignoreIdsString = null) {
			List<Guid> ignoreIds;
			if (string.IsNullOrEmpty(ignoreIdsString))
				ignoreIds = new List<Guid>();
			else
				ignoreIds = Array.ConvertAll(ignoreIdsString.Split(','), Guid.Parse).ToList();

			return await _mediator.Send(new GetFilesCommand(ignoreIds));
		}

		[HttpPost("start")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Start([FromBody] StartFileUploadCommand command) => await _mediator.Send(command);

		[HttpPost("{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Upload([FromForm] IFormFile file, Guid id) {
			var command = new FileUploadCommand {
				File = file,
				FileId = id
			};
			return await _mediator.Send(command);
		}

		[HttpPost("convert")]
		[ProducesResponseType((int)HttpStatusCode.Created)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Convert([FromBody] RequestConvertCommand command) => await _mediator.Send(command);

		[HttpGet("conversions")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> GetAvailableConversions([FromQuery] string mimeType) => await _mediator.Send(new GetAvailableConversionsCommand(HttpUtility.UrlDecode(mimeType)));

		[HttpPost("url")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> GetAuthorizationUrl([FromBody] CreateFileAuthTokenCommand command) => await _mediator.Send(command);
	}
}
