using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexiFile.Core.Interfaces.Services {
	public interface IFileService {
		public Task UploadFile(IFormFile file, Guid fileId, Guid userId);
	}
}
