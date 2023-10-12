using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Events;
using System.Threading.Channels;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Core.Interfaces.Services.ConvertServices {
	public interface IConvertFileService {
		Task ConvertFile(ChannelWriter<ConvertProgressNotificationEvent> notificationChannelWriter, File file, string fileDirectory, FileType inputFileType, FileType outputFileType);
	}
}
