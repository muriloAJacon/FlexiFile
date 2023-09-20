namespace FlexiFile.Application.ViewModels {
	public class GetFirstSetupViewModel {
		public bool FirstSetupPending { get; set; }

		public GetFirstSetupViewModel(bool firstSetupPending) {
			FirstSetupPending = firstSetupPending;
		}
	}
}
