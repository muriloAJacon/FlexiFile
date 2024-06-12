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
			var conversion = await _unitOfWork.FileConversionRepository.GetByIdAsync(request.ConversionId) ?? throw new Exception("Failed to find conversion");

			conversion.User.StorageUsed += request.Size;
			var result = new FileConversionResult {
				Id = request.FileId,
				TypeId = request.TypeId,
				FileConversionId = request.ConversionId,
				Size = request.Size,
				Order = request.Order,
				CreationDate = DateTime.UtcNow
			};

			_unitOfWork.FileConversionResultRepository.Add(result);
			await _unitOfWork.Commit();
		}
	}
}
