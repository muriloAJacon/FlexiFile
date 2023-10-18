using FlexiFile.Application.Commands.SettingsCommands.ChangeAllowAnonymousRegister;
using FlexiFile.Application.Commands.SettingsCommands.ChangeGlobalMaximumFileSize;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlexiFile.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[Authorize]
	[ApiController]
	public class SettingsController : ControllerBase {
		private readonly IMediator _mediator;

		public SettingsController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpPut("allowAnonymousRegister")]
		// TODO: Validate access level
		public async Task<IActionResult> ChangeAllowAnonymousRegister([FromBody] ChangeAllowAnonymousRegisterCommand command) => await _mediator.Send(command);

		[HttpPut("globalMaximumFileSize")]
		// TODO: Validate access level
		public async Task<IActionResult> ChangeGlobalMaximumFileSize([FromBody] ChangeGlobalMaximumFileSizeCommand command) => await _mediator.Send(command);
	}
}
