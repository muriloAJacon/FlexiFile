namespace FlexiFile.Application.ViewModels.FileTypeConversionViewModels {
	public class FileTypeConversionViewModel {
		public int Id { get; set; }
		public int ToTypeId { get; set; }
		public string Description { get; set; } = null!;
	}
}
