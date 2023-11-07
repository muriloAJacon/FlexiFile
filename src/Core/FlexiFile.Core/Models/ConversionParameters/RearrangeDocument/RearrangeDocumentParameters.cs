namespace FlexiFile.Core.Models.ConversionParameters.RearrangeDocument {
	public record RearrangeDocumentParameters {
		public List<int> OriginalPageNumbers { get; init; } = null!;
	}
}
