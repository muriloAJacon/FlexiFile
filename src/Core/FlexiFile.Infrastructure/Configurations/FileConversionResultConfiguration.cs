using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class FileConversionResultConfiguration : IEntityTypeConfiguration<FileConversionResult> {
		public void Configure(EntityTypeBuilder<FileConversionResult> builder) {
			builder.HasKey(e => e.Id).HasName("FileConversionResult_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.FileConversion).WithMany(p => p.FileConversionResults)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FileConversionResult_FileConversion_id_fk");

			builder.HasOne(d => d.Type).WithMany(p => p.FileConversionResults)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FileConversionResult_FileType_id_fk");
		}
	}
}