namespace FlexiFile.Core.Models.ConversionParameters.RearrangeDocument {
	public record RearrangeDocumentParameters {
		public List<int> OriginalIndexes { get; init; } = null!;
	}
}
