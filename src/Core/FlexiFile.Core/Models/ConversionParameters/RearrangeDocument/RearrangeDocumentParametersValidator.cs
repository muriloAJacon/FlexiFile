using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Core.Models.ConversionParameters.RearrangeDocument {
	public class RearrangeDocumentParametersValidator : AbstractValidator<RearrangeDocumentParameters> {
		public RearrangeDocumentParametersValidator() {
			RuleFor(x => x.OriginalPageNumbers)
				.NotEmpty()
				.ForEach(x => x.GreaterThan(0));
		}
	}
}
