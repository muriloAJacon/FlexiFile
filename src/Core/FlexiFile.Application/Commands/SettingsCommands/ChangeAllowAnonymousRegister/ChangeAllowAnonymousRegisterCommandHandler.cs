﻿using FlexiFile.Application.Results;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace FlexiFile.Application.Commands.SettingsCommands.ChangeAllowAnonymousRegister {
	public class ChangeAllowAnonymousRegisterCommandHandler : IRequestHandler<ChangeAllowAnonymousRegisterCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly DatabaseSettingsKeys _settingsKeys;

		public ChangeAllowAnonymousRegisterCommandHandler(IUnitOfWork unitOfWork, IOptions<DatabaseSettingsKeys> settingsKeys) {
			_unitOfWork = unitOfWork;
			_settingsKeys = settingsKeys.Value;
		}

		public async Task<IResultCommand> Handle(ChangeAllowAnonymousRegisterCommand request, CancellationToken cancellationToken) {
			var setting = await _unitOfWork.SettingRepository.GetSetting(_settingsKeys.AllowAnonymousRegisterKey);

			bool currentValue = Convert.ToBoolean(setting.Value);
			if (currentValue == request.AllowAnonymousRegister) {
				return ResultCommand.BadRequest("The value is already set to the requested value", "valueEqualToRequested");
			}

			setting.Value = request.AllowAnonymousRegister.ToString();
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
