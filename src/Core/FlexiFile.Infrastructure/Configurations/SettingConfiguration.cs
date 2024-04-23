using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class SettingConfiguration : IEntityTypeConfiguration<Setting> {
		public void Configure(EntityTypeBuilder<Setting> builder) {
			builder.HasKey(e => e.Id).HasName("pk_Setting");

			builder.HasOne(d => d.UpdatedByUser).WithMany(p => p.Settings).HasConstraintName("fk_Setting_User");

			builder.HasData(
				new Setting { Id = "GLOBAL_MAXIMUM_FILE_SIZE", Value = 0.ToString(), LastUpdateDate = null, UpdatedByUserId = null },
				new Setting { Id = "ALLOW_ANONYMOUS_REGISTER", Value = false.ToString(), LastUpdateDate = null, UpdatedByUserId = null }
			);
		}
	}
}