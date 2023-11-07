using FlexiFile.Application.Results;
using FlexiFile.Application.ViewModels.FileTypeConversionViewModels;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using FlexiFile.Core.Interfaces.Results;
using MediatR;

namespace FlexiFile.Application.Commands.ConvertCommands.GetAvailableConversionsCommand {
	public class GetAvailableConversionsCommandHandler : IRequestHandler<GetAvailableConversionsCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAvailableConversionsCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task<IResultCommand> Handle(GetAvailableConversionsCommand request, CancellationToken cancellationToken) {
			var conversions = await _unitOfWork.FileTypeConversionRepository.GetAvailableConversions(request.FromMimeType);

			return ResultCommand.Ok<List<FileTypeConversion>, List<FileTypeConversionViewModel>>(conversions);
		}
	}
}
