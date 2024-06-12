namespace FlexiFile.Core.Events {
	public class ConvertFileResultEvent : EventArgs {
		public Guid EventId { get; set; }
		public int TypeId { get; set; }
		public Guid FileId { get; set; }
		public long Size { get; set; }
		public int Order { get; set; }
	}
}
