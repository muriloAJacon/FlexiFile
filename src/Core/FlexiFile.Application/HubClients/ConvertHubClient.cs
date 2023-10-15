using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexiFile.Core.Models.Hubs.ConvertHub;
using FlexiFile.Core.OptionsBuilder;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace FlexiFile.Application.HubClients {
	public class ConvertHubClient {
		private readonly ILogger<ConvertHubClient> _logger;
		private readonly HubConnection _hubConnection;

		public ConvertHubClient(SignalRClientOptionsBuilder options, ILogger<ConvertHubClient> logger) {
			_logger = logger;
			_hubConnection = new HubConnectionBuilder()
				.WithUrl($"{options.ConnectionString}/convert")
				.WithAutomaticReconnect()
				.Build();

			_hubConnection.StartAsync().ContinueWith(_ => {
				_logger.LogInformation("Connected to hub");
			});
		}

		public async Task SendFileConvertRequestedAsync(FileConvertRequestedInfo info) {
			_logger.LogInformation("Sending FileConvertRequested for id {id}", info.ConversionId);
			await _hubConnection.InvokeAsync("FileConvertRequested", info);
			_logger.LogInformation("Successfully notified hub of convert requested for id {id}", info.ConversionId);
		}
	}
}
