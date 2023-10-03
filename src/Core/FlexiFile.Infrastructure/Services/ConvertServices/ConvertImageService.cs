using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Core.Interfaces.Services.ConvertServices;
using ImageMagick;

namespace FlexiFile.Infrastructure.Services.ConvertServices {
	public class ConvertImageService : IConvertImageService {
		public async Task ConvertFile(string fileDirectory, FileType inputFileType, FileType outputFileType) {
			try {
				var directory = new DirectoryInfo(fileDirectory);

				string inputFilePath = Path.Combine(directory.FullName, "original");

				Guid outputFileId = Guid.NewGuid();
				string outputPath = Path.Combine(directory.FullName, outputFileId.ToString());

				using var image = new MagickImage(inputFilePath);

				MagickFormat format = outputFileType.MimeTypes.First() switch {
					"image/jpeg" => MagickFormat.Jpeg,
					"image/png" => MagickFormat.Png,
					"image/gif" => MagickFormat.Gif,
					"image/tiff" => MagickFormat.Tiff,
					"image/vnd.microsoft.icon" => MagickFormat.Ico,
					"image/svg+xml" => MagickFormat.Svg,
					_ => throw new Exception("Mime type not supported")
				};

				if (format == MagickFormat.Ico) {
					image.Resize(16, 16); // TODO: IMPLEMENT BETTER
				}

				await image.WriteAsync(outputPath, format);
			} catch (Exception e) {
				Console.WriteLine(e);
			}
		}
	}
}
