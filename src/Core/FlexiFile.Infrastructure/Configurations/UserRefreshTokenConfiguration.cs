using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken> {
		public void Configure(EntityTypeBuilder<UserRefreshToken> builder) {
			builder.HasKey(e => e.Id).HasName("pk_UserRefreshToken");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.User).WithMany(p => p.UserRefreshTokens)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_UserRefreshToken_User");
		}
	}
}
