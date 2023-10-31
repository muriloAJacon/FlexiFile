using System;
using System.Collections.Generic;
using FlexiFile.Core.Entities.Postgres;
using Microsoft.EntityFrameworkCore;

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
        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_File");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.OwnedByUser).WithMany(p => p.Files)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_File_User");

            entity.HasOne(d => d.Type).WithMany(p => p.Files)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("File_FileType_id_fk");
        });

        modelBuilder.Entity<FileConversion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FileConversion_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasComment("(\"InQueue\",\"InProgress\",\"Completed\",\"Failed\")");

            entity.HasOne(d => d.FileTypeConversion).WithMany(p => p.FileConversions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_FileConversion_FileTypeConversion");
        });

        modelBuilder.Entity<FileConversionOrigin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_FileConversionOrigin");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FileConversion).WithMany(p => p.FileConversionOrigins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_FileConversionOrigin_FileConversion");

            entity.HasOne(d => d.File).WithMany(p => p.FileConversionOrigins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_FileConversionOrigin_File");
        });

        modelBuilder.Entity<FileConversionResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FileConversionResult_pk");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FileConversion).WithMany(p => p.FileConversionResults)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FileConversionResult_FileConversion_id_fk");
        });

        modelBuilder.Entity<FileType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FileType_pk");
        });

        modelBuilder.Entity<FileTypeConversion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_FileTypeConversion");

            entity.Property(e => e.Type).HasComment("(\"Conversion\",\"Processing\")");

            entity.HasOne(d => d.FromType).WithMany(p => p.FileTypeConversionFromTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_FileTypeConversion_FileType_from_type");

            entity.HasOne(d => d.ToType).WithMany(p => p.FileTypeConversionToTypes).HasConstraintName("fk_FileTypeConversion_FileType_to_type");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Setting");

            entity.HasOne(d => d.UpdatedByUser).WithMany(p => p.Settings).HasConstraintName("fk_Setting_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_User");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.ApprovedByUser).WithMany(p => p.InverseApprovedByUser).HasConstraintName("fk_User_User_approvedby");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.InverseCreatedByUser).HasConstraintName("fk_User_User_createdby");
        });

        modelBuilder.Entity<UserLoginAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_UserLoginAudit");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.UserLoginAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserLoginAudit_User");
        });

        modelBuilder.Entity<UserRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_UserRefreshToken");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.UserRefreshTokens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserRefreshToken_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
