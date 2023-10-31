using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("FileConversionOrigin", Schema = "FlexiFile")]
public partial class FileConversionOrigin
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("file_conversion_id")]
    public Guid FileConversionId { get; set; }

    [Column("file_id")]
    public Guid FileId { get; set; }

    [Column("order")]
    public int Order { get; set; }

    [Column("extra_info", TypeName = "json")]
    public string? ExtraInfo { get; set; }

    [ForeignKey("FileId")]
    [InverseProperty("FileConversionOrigins")]
    public virtual File File { get; set; } = null!;

    [ForeignKey("FileConversionId")]
    [InverseProperty("FileConversionOrigins")]
    public virtual FileConversion FileConversion { get; set; } = null!;
}
