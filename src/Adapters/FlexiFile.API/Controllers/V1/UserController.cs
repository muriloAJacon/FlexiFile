using FlexiFile.Application.Commands.UserCommands.ApproveUser;
using FlexiFile.Application.Commands.UserCommands.CreateFirstSetup;
using FlexiFile.Application.Commands.UserCommands.CreateUser;
using FlexiFile.Application.Commands.UserCommands.EditUser;
using FlexiFile.Application.Commands.UserCommands.GetFirstSetup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlexiFile.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class UserController : ControllerBase {
		private readonly IMediator _mediator;

		public UserController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpPost("firstSetup")]
		[AllowAnonymous]
		public async Task<IActionResult> CreateFirstSetup([FromBody] CreateFirstSetupCommand command) => await _mediator.Send(command);

		[HttpGet("firstSetup")]
		[AllowAnonymous]
		public async Task<IActionResult> GetFirstSetup() => await _mediator.Send(new GetFirstSetupCommand());

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command) => await _mediator.Send(command);

		[HttpPut]
		[Authorize]
		// TODO: Validate access level
		public async Task<IActionResult> EditUser([FromBody] EditUserCommand command) => await _mediator.Send(command);

		[HttpPut("approve")]
		[Authorize]
		// TODO: Validate access level
		public async Task<IActionResult> ApproveUser([FromBody] ApproveUserCommand command) => await _mediator.Send(command);
	}
}
