namespace FlexiFile.Application.ViewModels {
	public class GetAllowAnonymousRegisterViewModel {
		public bool AnonymousRegisterAllowed { get; set; }

		public GetAllowAnonymousRegisterViewModel(bool anonymousRegisterAllowed) {
			AnonymousRegisterAllowed = anonymousRegisterAllowed;
		}
	}
}
