using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using FlexiFile.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("FileConversion", Schema = "FlexiFile")]
[Index("FileTypeConversionId", Name = "IX_FileConversion_file_type_conversion_id")]
public partial class FileConversion
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("file_type_conversion_id")]
    public int FileTypeConversionId { get; set; }

    [Column("status")]
    public ConvertStatus Status { get; set; }

    [Column("percentage_complete")]
    public double PercentageComplete { get; set; }

    [Column("creation_date")]
    [Precision(3, 0)]
    public DateTime CreationDate { get; set; }

    [Column("last_update_date")]
    [Precision(3, 0)]
    public DateTime LastUpdateDate { get; set; }

    [Column("extra_info", TypeName = "json")]
    public JsonElement? ExtraInfo { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [InverseProperty("FileConversion")]
    public virtual ICollection<FileConversionOrigin> FileConversionOrigins { get; set; } = new List<FileConversionOrigin>();

    [InverseProperty("FileConversion")]
    public virtual ICollection<FileConversionResult> FileConversionResults { get; set; } = new List<FileConversionResult>();

    [ForeignKey("FileTypeConversionId")]
    [InverseProperty("FileConversions")]
    public virtual FileTypeConversion FileTypeConversion { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("FileConversions")]
    public virtual User User { get; set; } = null!;
}
