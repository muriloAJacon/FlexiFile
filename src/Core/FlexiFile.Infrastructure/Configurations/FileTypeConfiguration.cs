using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class FileTypeConfiguration : IEntityTypeConfiguration<FileType> {
		public void Configure(EntityTypeBuilder<FileType> builder) {
			builder.HasKey(e => e.Id).HasName("FileType_pk");

			builder.HasData(
				new FileType { Id = 1, Description = "PNG", MimeType = "image/png", Extension = "png" },
				new FileType { Id = 2, Description = "JPEG", MimeType = "image/jpeg", Extension = "jpeg" },
				new FileType { Id = 3, Description = "ICO", MimeType = "image/vnd.microsoft.icon", Extension = "ico" },
				new FileType { Id = 4, Description = "SVG", MimeType = "image/svg+xml", Extension = "svg" },
				new FileType { Id = 5, Description = "GIF", MimeType = "image/gif", Extension = "gif" },
				new FileType { Id = 6, Description = "TIFF", MimeType = "image/tiff", Extension = "tiff" },
				new FileType { Id = 7, Description = "MP4", MimeType = "video/mp4", Extension = "mp4" },
				new FileType { Id = 8, Description = "MPEG", MimeType = "video/mpeg", Extension = "mpeg" },
				new FileType { Id = 9, Description = "WEBM", MimeType = "video/webm", Extension = "webm" },
				new FileType { Id = 10, Description = "MKV", MimeType = "video/x-matroska", Extension = "mkv" },
				new FileType { Id = 11, Description = "MP3", MimeType = "audio/mpeg", Extension = "mp3" },
				new FileType { Id = 12, Description = "M4A", MimeType = "audio/mp4", Extension = "m4a" },
				new FileType { Id = 13, Description = "PDF", MimeType = "application/pdf", Extension = "pdf" }
			);
		}
	}
}