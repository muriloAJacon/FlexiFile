using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class FileConversionConfiguration : IEntityTypeConfiguration<FileConversion> {
		public void Configure(EntityTypeBuilder<FileConversion> builder) {
			builder.HasKey(e => e.Id).HasName("FileConversion_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();
			builder.Property(e => e.Status).HasComment("(\"InQueue\",\"InProgress\",\"Completed\",\"Failed\")");

			builder.HasOne(d => d.FileTypeConversion).WithMany(p => p.FileConversions)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_FileConversion_FileTypeConversion");

			builder.HasOne(d => d.User).WithMany(p => p.FileConversions)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_FileConversion_User");
		}
	}
}
