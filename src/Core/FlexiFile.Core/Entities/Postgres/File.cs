using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("File", Schema = "FlexiFile")]
public partial class File
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("type_id")]
    public int TypeId { get; set; }

    [Column("owned_by_user_id")]
    public Guid OwnedByUserId { get; set; }

    [Column("size")]
    public long Size { get; set; }

    [Column("original_name", TypeName = "character varying")]
    public string OriginalName { get; set; } = null!;

    [Column("submitted_at", TypeName = "timestamp(3) with time zone")]
    public DateTime SubmittedAt { get; set; }

    [Column("finished_upload")]
    public bool FinishedUpload { get; set; }

    [Column("finished_upload_at", TypeName = "timestamp(3) with time zone")]
    public DateTime? FinishedUploadAt { get; set; }

    [InverseProperty("File")]
    public virtual ICollection<FileConversion> FileConversions { get; set; } = new List<FileConversion>();

    [ForeignKey("OwnedByUserId")]
    [InverseProperty("Files")]
    public virtual User OwnedByUser { get; set; } = null!;

    [ForeignKey("TypeId")]
    [InverseProperty("Files")]
    public virtual FileType Type { get; set; } = null!;
}
