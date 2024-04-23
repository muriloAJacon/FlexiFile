using System;
using System.Collections.Generic;
using FlexiFile.Core.Entities.Postgres;
using FlexiFile.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using File = FlexiFile.Core.Entities.Postgres.File;

namespace FlexiFile.Infrastructure.Context;

public partial class PostgresContext : DbContext
{
    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<FileConversion> FileConversions { get; set; }

    public virtual DbSet<FileConversionOrigin> FileConversionOrigins { get; set; }

    public virtual DbSet<FileConversionResult> FileConversionResults { get; set; }

    public virtual DbSet<FileType> FileTypes { get; set; }

    public virtual DbSet<FileTypeConversion> FileTypeConversions { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLoginAudit> UserLoginAudits { get; set; }

    public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new FileConfiguration());

        modelBuilder.ApplyConfiguration(new FileConversionConfiguration());

		modelBuilder.ApplyConfiguration(new FileConversionOriginConfiguration());

		modelBuilder.ApplyConfiguration(new FileConversionResultConfiguration());

		modelBuilder.ApplyConfiguration(new FileTypeConfiguration());

		modelBuilder.ApplyConfiguration(new FileTypeConversionConfiguration());

		modelBuilder.ApplyConfiguration(new SettingConfiguration());

		modelBuilder.ApplyConfiguration(new UserConfiguration());

		modelBuilder.ApplyConfiguration(new UserLoginAuditConfiguration());

		modelBuilder.ApplyConfiguration(new UserRefreshTokenConfiguration());

		OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
