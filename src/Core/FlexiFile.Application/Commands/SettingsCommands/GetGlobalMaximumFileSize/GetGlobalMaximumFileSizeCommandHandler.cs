using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels;
using FlexiFile.Application.ViewModels.SettingsViewModels;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.Commands.SettingsCommands.GetGlobalMaximumFileSize {
	public class GetGlobalMaximumFileSizeCommandHandler : IRequestHandler<GetGlobalMaximumFileSizeCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly DatabaseSettingsKeys _settingsKeys;

		public GetGlobalMaximumFileSizeCommandHandler(IUnitOfWork unitOfWork, IOptions<DatabaseSettingsKeys> settingsKeys) {
			_unitOfWork = unitOfWork;
			_settingsKeys = settingsKeys.Value;
		}

		public async Task<IResultCommand> Handle(GetGlobalMaximumFileSizeCommand request, CancellationToken cancellationToken) {
			var setting = await _unitOfWork.SettingRepository.GetSetting(_settingsKeys.GlobalMaximumFileSizeKey);

			long value = Convert.ToInt64(setting.Value);

			return ResultCommand.Ok<GetGlobalMaximumFileSizeViewModel, GetGlobalMaximumFileSizeViewModel>(new GetGlobalMaximumFileSizeViewModel(value));
		}
	}
}
