using FlexiFile.Core.Entities.Postgres;

namespace FlexiFile.Core.Interfaces.Services.ConvertServices {
	public interface IConvertFileService {
		Task ConvertFile(string fileDirectory, FileType inputFileType, FileType outputFileType);
	}
}
