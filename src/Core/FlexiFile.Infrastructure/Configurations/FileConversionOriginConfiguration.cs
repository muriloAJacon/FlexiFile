using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class FileConversionOriginConfiguration : IEntityTypeConfiguration<FileConversionOrigin> {
		public void Configure(EntityTypeBuilder<FileConversionOrigin> builder) {
			builder.HasKey(e => e.Id).HasName("pk_FileConversionOrigin");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.FileConversion).WithMany(p => p.FileConversionOrigins)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_FileConversionOrigin_FileConversion");

			builder.HasOne(d => d.File).WithMany(p => p.FileConversionOrigins)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_FileConversionOrigin_File");
		}
	}
}