using Asp.Versioning;
using FlexiFile.API.Filters;
using FlexiFile.Application.Commands.SettingsCommands.ChangeAllowAnonymousRegister;
using FlexiFile.Application.Commands.SettingsCommands.ChangeGlobalMaximumFileSize;
using FlexiFile.Application.Commands.SettingsCommands.GetAllowAnonymousRegister;
using FlexiFile.Application.Commands.SettingsCommands.GetGlobalMaximumFileSize;
using FlexiFile.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlexiFile.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class SettingsController : ControllerBase {
		private readonly IMediator _mediator;

		public SettingsController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpGet("allowAnonymousRegister")]
		public async Task<IActionResult> GetAllowAnonymousRegister() => await _mediator.Send(new GetAllowAnonymousRegisterCommand());

		[HttpPut("allowAnonymousRegister")]
		[Authorize]
		[AccessLevel(AccessLevel.Root)]
		public async Task<IActionResult> ChangeAllowAnonymousRegister([FromBody] ChangeAllowAnonymousRegisterCommand command) => await _mediator.Send(command);

		[HttpGet("globalMaximumFileSize")]
		[Authorize]
		[AccessLevel(AccessLevel.Root)]
		public async Task<IActionResult> GetGlobalMaximumFileSize() => await _mediator.Send(new GetGlobalMaximumFileSizeCommand());

		[HttpPut("globalMaximumFileSize")]
		[Authorize]
		[AccessLevel(AccessLevel.Root)]
		public async Task<IActionResult> ChangeGlobalMaximumFileSize([FromBody] ChangeGlobalMaximumFileSizeCommand command) => await _mediator.Send(command);
	}
}
