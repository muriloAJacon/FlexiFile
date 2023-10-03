using FlexiFile.Application.Commands.ConvertCommands.StartConvertCommand;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace FlexiFile.Worker.Hubs {
	public class ConvertHub : Hub {

		private readonly ILogger<ConvertHub> _logger;
		private readonly IMediator _mediator;

		public ConvertHub(ILogger<ConvertHub> logger, IMediator mediator) {
			_logger = logger;
			_mediator = mediator;
		}

		public async Task FileConvertRequested(string idStr) {
			_logger.LogInformation("FileConvertRequested called from client {clientId} with parameters idStr: {id}", Context.ConnectionId, idStr);

			Guid id = Guid.Parse(idStr);

			await _mediator.Send(new StartConvertCommand {
				ConversionId = id
			});
		}

		public override Task OnConnectedAsync() {
			_logger.LogInformation("Client {id} connected", Context.ConnectionId);

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception) {
			if (exception is null) {
				_logger.LogInformation("Client {id} connected", Context.ConnectionId);
			} else {
				_logger.LogError(exception, "Client {id} disconnected with exception", Context.ConnectionId);
			}

			return base.OnDisconnectedAsync(exception);
		}
	}
}
