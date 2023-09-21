namespace FlexiFile.Application.ViewModels.FileViewModels {
	public class FileViewModel {
		public Guid Id { get; set; }
		public string TypeDescription { get; set; } = null!;
		public long Size { get; set; }
		public string OriginalName { get; set; } = null!;
		public DateTime SubmittedAt { get; set; }
		public bool FinishedUpload { get; set; }
		public DateTime? FinishedUploadAt { get; set; }
	}
}
