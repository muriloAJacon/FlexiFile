using Asp.Versioning;
using FlexiFile.Application.Commands.AuthCommands.Login;
using FlexiFile.Application.Commands.AuthCommands.RequestRefreshToken;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FlexiFile.API.Controllers.V1 {
	[Route("api/v{version:apiVersion}/[controller]")]
	[ApiVersion("1.0")]
	[ApiController]
	public class AuthController : ControllerBase {
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator) {
			_mediator = mediator;
		}

		[HttpPost("login")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Login([FromBody] LoginCommand command) => await _mediator.Send(command);

		[HttpPost("refresh-token")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> RefreshToken([FromBody] RequestRefreshTokenCommand command) => await _mediator.Send(command);
	}
}
