using FlexiFile.Core.Enums;

namespace FlexiFile.Core.Events {
	public class ConvertProgressNotificationEvent : EventArgs {
		public Guid EventId { get; set; }
		public ConvertStatus ConvertStatus { get; set; }
		public double? PercentageComplete { get; set; }
	}
}
