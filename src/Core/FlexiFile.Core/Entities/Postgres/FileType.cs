using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("FileType", Schema = "FlexiFile")]
public partial class FileType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("description", TypeName = "character varying")]
    public string Description { get; set; } = null!;

    [Column("mime_type", TypeName = "character varying")]
    public string MimeType { get; set; } = null!;

    [Column("extension", TypeName = "character varying")]
    public string Extension { get; set; } = null!;

    [InverseProperty("FromType")]
    public virtual ICollection<FileTypeConversion> FileTypeConversionFromTypes { get; set; } = new List<FileTypeConversion>();

    [InverseProperty("ToType")]
    public virtual ICollection<FileTypeConversion> FileTypeConversionToTypes { get; set; } = new List<FileTypeConversion>();

    [InverseProperty("Type")]
    public virtual ICollection<File> Files { get; set; } = new List<File>();

    [InverseProperty("Type")]
    public virtual ICollection<FileConversionResult> FileConversionResults { get; set; } = new List<FileConversionResult>();
}
