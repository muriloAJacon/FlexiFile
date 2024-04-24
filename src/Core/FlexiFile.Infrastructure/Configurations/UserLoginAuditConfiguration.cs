using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class UserLoginAuditConfiguration : IEntityTypeConfiguration<UserLoginAudit> {
		public void Configure(EntityTypeBuilder<UserLoginAudit> builder) {
			builder.HasKey(e => e.Id).HasName("pk_UserLoginAudit");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.User).WithMany(p => p.UserLoginAudits)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_UserLoginAudit_User");
		}
	}
}