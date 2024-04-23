using System;
using System.Collections.Generic;
using FlexiFile.Core.Entities.Postgres;
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

            entity.HasOne(d => d.User).WithMany(p => p.FileConversions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_FileConversion_User");
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

            entity.HasData(
                new FileType { Id = 1, Description = "PNG", MimeType = "image/png" },
                new FileType { Id = 2, Description = "JPEG", MimeType = "image/jpeg" },
                new FileType { Id = 3, Description = "ICO", MimeType = "image/vnd.microsoft.icon" },
                new FileType { Id = 4, Description = "SVG", MimeType = "image/svg+xml" },
                new FileType { Id = 5, Description = "GIF", MimeType = "image/gif" },
                new FileType { Id = 6, Description = "TIFF", MimeType = "image/tiff" },
                new FileType { Id = 7, Description = "MP4", MimeType = "video/mp4" },
                new FileType { Id = 8, Description = "MPEG", MimeType = "video/mpeg" },
                new FileType { Id = 9, Description = "WEBM", MimeType = "video/webm" },
                new FileType { Id = 10, Description = "MKV", MimeType = "video/x-matroska" },
                new FileType { Id = 11, Description = "MP3", MimeType = "audio/mpeg" },
                new FileType { Id = 12, Description = "M4A", MimeType = "audio/mp4" },
                new FileType { Id = 13, Description = "PDF", MimeType = "application/pdf" }
            );
        });

        modelBuilder.Entity<FileTypeConversion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_FileTypeConversion");

            entity.Property(e => e.Type).HasComment("(\"Conversion\",\"Processing\")");

            entity.HasOne(d => d.FromType).WithMany(p => p.FileTypeConversionFromTypes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_FileTypeConversion_FileType_from_type");

            entity.HasOne(d => d.ToType).WithMany(p => p.FileTypeConversionToTypes).HasConstraintName("fk_FileTypeConversion_FileType_to_type");

			entity.HasData(
				new FileTypeConversion { Id = 1, Type = "1", FromTypeId = 1, ToTypeId = 2, Description = "Converts PNG to JPEG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 2, Type = "1", FromTypeId = 1, ToTypeId = 3, Description = "Converts PNG to ICO", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 3, Type = "1", FromTypeId = 1, ToTypeId = 4, Description = "Converts PNG to SVG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 4, Type = "1", FromTypeId = 10, ToTypeId = 7, Description = "Converts MKV to MP4", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 5, Type = "1", FromTypeId = 11, ToTypeId = 12, Description = "Converts MP3 to M4A", IsActive = true, HandlerClassName = "IConvertAudioService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 6, Type = "1", FromTypeId = 7, ToTypeId = 11, Description = "Converts MP4 to MP3", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 7, Type = "1", FromTypeId = 1, ToTypeId = 5, Description = "Converts PNG to GIF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 8, Type = "1", FromTypeId = 1, ToTypeId = 6, Description = "Converts PNG to TIFF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 9, Type = "1", FromTypeId = 2, ToTypeId = 1, Description = "Converts JPEG to PNG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 10, Type = "1", FromTypeId = 2, ToTypeId = 3, Description = "Converts JPEG to ICO", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 11, Type = "1", FromTypeId = 2, ToTypeId = 4, Description = "Converts JPEG to SVG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 12, Type = "1", FromTypeId = 2, ToTypeId = 5, Description = "Converts JPEG to GIF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 13, Type = "1", FromTypeId = 2, ToTypeId = 6, Description = "Converts JPEG to TIFF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 14, Type = "1", FromTypeId = 3, ToTypeId = 1, Description = "Converts ICO to PNG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 15, Type = "1", FromTypeId = 3, ToTypeId = 2, Description = "Converts ICO to JPEG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 16, Type = "1", FromTypeId = 3, ToTypeId = 4, Description = "Converts ICO to SVG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 17, Type = "1", FromTypeId = 3, ToTypeId = 5, Description = "Converts ICO to GIF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 18, Type = "1", FromTypeId = 3, ToTypeId = 6, Description = "Converts ICO to TIFF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 19, Type = "1", FromTypeId = 4, ToTypeId = 1, Description = "Converts SVG to PNG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 20, Type = "1", FromTypeId = 4, ToTypeId = 2, Description = "Converts SVG to JPEG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 21, Type = "1", FromTypeId = 4, ToTypeId = 3, Description = "Converts SVG to ICO", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 22, Type = "1", FromTypeId = 4, ToTypeId = 5, Description = "Converts SVG to GIF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 23, Type = "1", FromTypeId = 4, ToTypeId = 6, Description = "Converts SVG to TIFF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 24, Type = "1", FromTypeId = 5, ToTypeId = 1, Description = "Converts GIF to PNG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 25, Type = "1", FromTypeId = 5, ToTypeId = 2, Description = "Converts GIF to JPEG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 26, Type = "1", FromTypeId = 5, ToTypeId = 3, Description = "Converts GIF to ICO", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 27, Type = "1", FromTypeId = 5, ToTypeId = 4, Description = "Converts GIF to SVG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 28, Type = "1", FromTypeId = 5, ToTypeId = 6, Description = "Converts GIF to TIFF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 29, Type = "1", FromTypeId = 6, ToTypeId = 1, Description = "Converts TIFF to PNG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 30, Type = "1", FromTypeId = 6, ToTypeId = 2, Description = "Converts TIFF to JPEG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 31, Type = "1", FromTypeId = 6, ToTypeId = 3, Description = "Converts TIFF to ICO", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 32, Type = "1", FromTypeId = 6, ToTypeId = 4, Description = "Converts TIFF to SVG", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 33, Type = "1", FromTypeId = 6, ToTypeId = 5, Description = "Converts TIFF to GIF", IsActive = true, HandlerClassName = "IConvertImageService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 34, Type = "1", FromTypeId = 7, ToTypeId = 8, Description = "Converts MP4 to MPEG", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 35, Type = "1", FromTypeId = 7, ToTypeId = 9, Description = "Converts MP4 to WEBM", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 36, Type = "1", FromTypeId = 7, ToTypeId = 10, Description = "Converts MP4 to MKV", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 37, Type = "1", FromTypeId = 8, ToTypeId = 7, Description = "Converts MPEG to MP4", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 38, Type = "1", FromTypeId = 8, ToTypeId = 9, Description = "Converts MPEG to WEBM", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 39, Type = "1", FromTypeId = 8, ToTypeId = 10, Description = "Converts MPEG to MKV", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 40, Type = "1", FromTypeId = 9, ToTypeId = 7, Description = "Converts WEBM to MP4", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 41, Type = "1", FromTypeId = 9, ToTypeId = 8, Description = "Converts WEBM to MPEG", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 42, Type = "1", FromTypeId = 9, ToTypeId = 10, Description = "Converts WEBM to MKV", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 43, Type = "1", FromTypeId = 10, ToTypeId = 8, Description = "Converts MKV to MPEG", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 44, Type = "1", FromTypeId = 10, ToTypeId = 9, Description = "Converts MKV to WEBM", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 45, Type = "1", FromTypeId = 8, ToTypeId = 11, Description = "Converts MPEG to MP3", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 46, Type = "1", FromTypeId = 9, ToTypeId = 11, Description = "Converts WEBM to MP3", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 47, Type = "1", FromTypeId = 10, ToTypeId = 11, Description = "Converts MKV to MP3", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 48, Type = "1", FromTypeId = 7, ToTypeId = 12, Description = "Converts MP4 to M4A", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 49, Type = "1", FromTypeId = 8, ToTypeId = 12, Description = "Converts MPEG to M4A", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 50, Type = "1", FromTypeId = 9, ToTypeId = 12, Description = "Converts WEBM to M4A", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 51, Type = "1", FromTypeId = 10, ToTypeId = 12, Description = "Converts MKV to M4A", IsActive = true, HandlerClassName = "IConvertVideoService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 52, Type = "1", FromTypeId = 12, ToTypeId = 11, Description = "Converts M4A to MP3", IsActive = true, HandlerClassName = "IConvertAudioService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 53, Type = "2", FromTypeId = 13, ToTypeId = null, Description = "Splits one PDF file into multiple files", IsActive = true, HandlerClassName = "ISplitDocumentService", MinNumberFiles = null, MaxNumberFiles = 1 },
                new FileTypeConversion { Id = 54, Type = "2", FromTypeId = 13, ToTypeId = null, Description = "Merges multiple PDF files into one file", IsActive = true, HandlerClassName = "IMergeDocumentService", MinNumberFiles = 2, MaxNumberFiles = null },
                new FileTypeConversion { Id = 55, Type = "2", FromTypeId = 13, ToTypeId = null, Description = "Rearranges the pages of a PDF file", IsActive = true, HandlerClassName = "IRearrangeDocumentService", MinNumberFiles = null, MaxNumberFiles = 1, ModelClassName = "RearrangeDocumentParameters" }
			);
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
