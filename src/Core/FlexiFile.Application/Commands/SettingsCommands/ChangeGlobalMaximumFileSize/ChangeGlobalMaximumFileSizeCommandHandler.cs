using FlexiFile.Application.Results;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using FlexiFile.Core.Models.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace FlexiFile.Application.Commands.SettingsCommands.ChangeGlobalMaximumFileSize {
	public class ChangeGlobalMaximumFileSizeCommandHandler : IRequestHandler<ChangeGlobalMaximumFileSizeCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly DatabaseSettingsKeys _settingsKeys;

		public ChangeGlobalMaximumFileSizeCommandHandler(IUnitOfWork unitOfWork, IOptions<DatabaseSettingsKeys> settingsKeys) {
			_unitOfWork = unitOfWork;
			_settingsKeys = settingsKeys.Value;
		}

		public async Task<IResultCommand> Handle(ChangeGlobalMaximumFileSizeCommand request, CancellationToken cancellationToken) {
			var setting = await _unitOfWork.SettingRepository.GetSetting(_settingsKeys.GlobalMaximumFileSizeKey);

			long currentValue = Convert.ToInt64(setting.Value);

			long newValue = request.MaxFileSize ?? 0;

			if (currentValue == newValue) {
				return ResultCommand.BadRequest("The value is already set to the requested value", "valueEqualToRequested");
			}

			setting.Value = newValue.ToString();
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
