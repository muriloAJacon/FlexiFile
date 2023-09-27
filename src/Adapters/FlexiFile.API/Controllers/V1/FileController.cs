using FlexiFile.Application.Commands.ConvertCommands.RequestConvertCommand;
using FlexiFile.Application.Commands.FileCommands.FileUpload;
using FlexiFile.Application.Commands.FileCommands.StartFileUpload;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

		[HttpPost("start")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Start([FromBody] StartFileUploadCommand command) => await _mediator.Send(command);

		[HttpPost("{id:guid}")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Start([FromForm] IFormFile file, Guid id) {
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
	}
}
