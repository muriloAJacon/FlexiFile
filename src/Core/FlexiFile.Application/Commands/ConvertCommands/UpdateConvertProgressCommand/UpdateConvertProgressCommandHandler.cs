using FlexiFile.Application.Hubs;
using FlexiFile.Core.Interfaces.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.ConvertCommands.UpdateConvertProgressCommand {
	public class UpdateConvertProgressCommandHandler : IRequestHandler<UpdateConvertProgressCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHubContext<ConvertHub> _convertHubContext;

		public UpdateConvertProgressCommandHandler(IUnitOfWork unitOfWork, IHubContext<ConvertHub> convertHubContext) {
			_unitOfWork = unitOfWork;
			_convertHubContext = convertHubContext;
		}

		public async Task Handle(UpdateConvertProgressCommand notification, CancellationToken cancellationToken) {
			var fileConversion = await _unitOfWork.FileConversionRepository.GetByIdAsync(notification.ConversionId) ?? throw new Exception("File Conversion not found");

			fileConversion.Status = notification.ConvertStatus;
			if (notification.PercentageComplete is not null) {
				fileConversion.PercentageComplete = notification.PercentageComplete.Value;
			}
			fileConversion.LastUpdateDate = DateTime.UtcNow;

			await _unitOfWork.Commit();

			await _convertHubContext.Clients.All.SendAsync("FileConvertProgress", fileConversion, cancellationToken);
		}
	}
}
