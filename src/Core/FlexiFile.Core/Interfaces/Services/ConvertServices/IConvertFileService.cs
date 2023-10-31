using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using System.Threading.Channels;

namespace FlexiFile.Core.Interfaces.Services.ConvertServices {
	public interface IConvertFileService {
		Task ConvertFile(ChannelWriter<EventArgs> notificationChannelWriter, FileConversion fileConversion, string fileDirectory, FileType inputFileType, FileType? outputFileType);
	}
}
