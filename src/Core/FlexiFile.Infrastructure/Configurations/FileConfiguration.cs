using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Infrastructure.Configurations {
	public class FileConfiguration : IEntityTypeConfiguration<File> {
		public void Configure(EntityTypeBuilder<File> builder) {
			builder.HasKey(e => e.Id).HasName("pk_File");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.OwnedByUser).WithMany(p => p.Files)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_File_User");

			builder.HasOne(d => d.Type).WithMany(p => p.Files)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("File_FileType_id_fk");
		}
	}
}
