using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Repository;
using MediatR;

namespace FlexiFile.Application.Commands.FileCommands.CreateFileConvertResult {
	public class CreateFileConvertResultCommandHandler : IRequestHandler<CreateFileConvertResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public CreateFileConvertResultCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork;
		}

		public async Task Handle(CreateFileConvertResultCommand request, CancellationToken cancellationToken) {
			var result = new FileConversionResult {
				Id = request.FileId,
				FileConversionId = request.ConversionId,
				Order = request.Order,
				CreationDate = DateTime.UtcNow
			};

			_unitOfWork.FileConversionResultRepository.Add(result);
			await _unitOfWork.Commit();
		}
	}
}
