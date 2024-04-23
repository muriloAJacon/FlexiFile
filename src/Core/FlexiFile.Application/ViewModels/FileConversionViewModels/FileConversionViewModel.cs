using FlexiFile.Application.ViewModels.FileViewModels;
using FlexiFile.Core.Enums;

namespace FlexiFile.Application.ViewModels.FileConversionViewModels {
	public class FileConversionViewModel {
		public Guid Id { get; set; }
		public ConvertStatus Status { get; set; }
		public double PercentageComplete { get; set; }
		public DateTime CreationDate { get; set; }
		public List<FileConversionResultViewModel> FileResults { get; set; } = null!;
	}
}
