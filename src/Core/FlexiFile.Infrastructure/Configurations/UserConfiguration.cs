using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlexiFile.Infrastructure.Configurations {
	public class UserConfiguration : IEntityTypeConfiguration<User> {
		public void Configure(EntityTypeBuilder<User> builder) {
			builder.HasKey(e => e.Id).HasName("pk_User");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.ApprovedByUser).WithMany(p => p.InverseApprovedByUser).HasConstraintName("fk_User_User_approvedby");

			builder.HasOne(d => d.CreatedByUser).WithMany(p => p.InverseCreatedByUser).HasConstraintName("fk_User_User_createdby");
		}
	}
}
