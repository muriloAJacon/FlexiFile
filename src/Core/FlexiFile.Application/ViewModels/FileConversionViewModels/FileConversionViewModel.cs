using FlexiFile.Application.ViewModels.FileViewModels;

namespace FlexiFile.Application.ViewModels.FileConversionViewModels {
	public class FileConversionViewModel {
		public Guid Id { get; set; }
		public FileViewModel File { get; set; } = null!;
	}
}
