﻿using FlexiFile.API.Filters;
using FlexiFile.Application.Commands.SettingsCommands.ChangeAllowAnonymousRegister;
using FlexiFile.Application.Commands.SettingsCommands.ChangeGlobalMaximumFileSize;
using FlexiFile.Core.Enums;
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
		[AccessLevel(AccessLevel.Root)]
		public async Task<IActionResult> ChangeAllowAnonymousRegister([FromBody] ChangeAllowAnonymousRegisterCommand command) => await _mediator.Send(command);

		[HttpPut("globalMaximumFileSize")]
		[AccessLevel(AccessLevel.Root)]
		public async Task<IActionResult> ChangeGlobalMaximumFileSize([FromBody] ChangeGlobalMaximumFileSizeCommand command) => await _mediator.Send(command);
	}
}
