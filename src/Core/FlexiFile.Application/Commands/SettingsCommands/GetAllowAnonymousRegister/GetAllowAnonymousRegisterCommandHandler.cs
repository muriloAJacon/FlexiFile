using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Core.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace FlexiFile.Application.Commands.SettingsCommands.GetAllowAnonymousRegister {
	public class GetAllowAnonymousRegisterCommandHandler : IRequestHandler<GetAllowAnonymousRegisterCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly DatabaseSettingsKeys _settingsKeys;

		public GetAllowAnonymousRegisterCommandHandler(IUnitOfWork unitOfWork, IOptions<DatabaseSettingsKeys> settingsKeys) {
			_unitOfWork = unitOfWork;
			_settingsKeys = settingsKeys.Value;
		}

		public async Task<IResultCommand> Handle(GetAllowAnonymousRegisterCommand request, CancellationToken cancellationToken) {
			var setting = await _unitOfWork.SettingRepository.GetSetting(_settingsKeys.AllowAnonymousRegisterKey);

			bool value = Convert.ToBoolean(setting.Value);

			return ResultCommand.Ok<GetAllowAnonymousRegisterViewModel, GetAllowAnonymousRegisterViewModel>(new GetAllowAnonymousRegisterViewModel(value));
		}
	}
}
