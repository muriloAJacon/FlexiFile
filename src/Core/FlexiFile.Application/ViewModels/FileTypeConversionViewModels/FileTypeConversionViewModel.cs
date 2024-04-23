namespace FlexiFile.Application.ViewModels.FileTypeConversionViewModels {
	public class FileTypeConversionViewModel {
		public int Id { get; set; }
		public string? ToTypeDescription { get; set; }
		public string Description { get; set; } = null!;
		public int? MinNumberFiles { get; set; }
		public int? MaxNumberFiles { get; set; }
		public string? ModelClassName { get; set; }
	}
}
