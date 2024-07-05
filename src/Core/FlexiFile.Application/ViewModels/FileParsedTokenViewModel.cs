using FlexiFile.Core.Enums;

namespace FlexiFile.Application.ViewModels {
	public class FileParsedTokenViewModel {
		public Guid FileId { get; set; }

		public string FilePath { get; set; } = null!;

		public string FileName { get; set; } = null!;

		public string FileType { get; set; }
	}
}
