﻿namespace FlexiFile.Core.Events {
	public class ConvertFileResultEvent : EventArgs {
		public Guid EventId { get; set; }
		public Guid FileId { get; set; }
		public int Order { get; set; }
	}
}
