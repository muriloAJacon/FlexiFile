namespace FlexiFile.Application.Results {
	public static class ValidatorsModelErrorMessages {
		public const string MaxLength = "This property exceeds the maximum length allowed.";
		public const string NullOrEmpty = "This property cannot be null or empty.";
		public const string Null = "This property cannot be null.";
		public const string MinLength = "This property does not meet the minimum length requirement.";
		public const string Email = "This property is not a valid email address.";
		public const string URL = "This property is not a valid URL.";
		public const string PasswordNumbers = "The password must contain at least one number.";
		public const string PasswordCapitalLetters = "The password must contain at least one capital letter.";
		public const string MinValue = "This property value is below the minimum allowed.";
		public const string MaxValue = "This property value exceeds the maximum allowed.";
	}
}