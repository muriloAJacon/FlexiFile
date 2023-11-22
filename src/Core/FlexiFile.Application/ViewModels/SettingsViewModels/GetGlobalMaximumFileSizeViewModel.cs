using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Application.ViewModels.SettingsViewModels {
	public class GetGlobalMaximumFileSizeViewModel {
		public long MaxFileSize { get; set; }

		public GetGlobalMaximumFileSizeViewModel(long maxFileSize) {
			MaxFileSize = maxFileSize;
		}
	}
}
