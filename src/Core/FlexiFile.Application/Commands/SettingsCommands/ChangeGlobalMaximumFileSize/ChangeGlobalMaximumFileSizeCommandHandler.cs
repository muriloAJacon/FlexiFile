using FlexiFile.Application.Results;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Interfaces.Services;
using FlexiFile.Core.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace FlexiFile.Application.Commands.SettingsCommands.ChangeGlobalMaximumFileSize {
	public class ChangeGlobalMaximumFileSizeCommandHandler : IRequestHandler<ChangeGlobalMaximumFileSizeCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly DatabaseSettingsKeys _settingsKeys;
		private readonly IUserClaimsService _userClaimsService;

		public ChangeGlobalMaximumFileSizeCommandHandler(IUnitOfWork unitOfWork, IOptions<DatabaseSettingsKeys> settingsKeys, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork;
			_settingsKeys = settingsKeys.Value;
			_userClaimsService = userClaimsService;
		}

		public async Task<IResultCommand> Handle(ChangeGlobalMaximumFileSizeCommand request, CancellationToken cancellationToken) {
			var setting = await _unitOfWork.SettingRepository.GetSetting(_settingsKeys.GlobalMaximumFileSizeKey);

			long currentValue = Convert.ToInt64(setting.Value);

			long newValue = request.MaxFileSize;

			if (currentValue == newValue) {
				return ResultCommand.BadRequest("The value is already set to the requested value", "valueEqualToRequested");
			}

			setting.Value = newValue.ToString();
			setting.LastUpdateDate = DateTime.UtcNow;
			setting.UpdatedByUserId = _userClaimsService.Id;

			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
