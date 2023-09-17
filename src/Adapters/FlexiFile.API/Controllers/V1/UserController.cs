using FlexiFile.Application.Commands.UserCommands.CreateFirstSetup;
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
	}
}
