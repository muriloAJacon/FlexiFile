using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("FileConversion", Schema = "FormatFlex")]
public partial class FileConversion
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("file_id")]
    public Guid FileId { get; set; }

    [Column("file_type_conversion_id")]
    public int FileTypeConversionId { get; set; }

    /// <summary>
    /// (&quot;InQueue&quot;,&quot;InProgress&quot;,&quot;Completed&quot;,&quot;Failed&quot;)
    /// </summary>
    [Column("status")]
    public int Status { get; set; }

    [Column("percentage_complete")]
    public double PercentageComplete { get; set; }

    [Column("creation_date", TypeName = "timestamp(3) without time zone")]
    public DateTime CreationDate { get; set; }

    [Column("last_update_date", TypeName = "timestamp(3) without time zone")]
    public DateTime LastUpdateDate { get; set; }

    [Column("extra_info", TypeName = "json")]
    public string? ExtraInfo { get; set; }

    [ForeignKey("FileId")]
    [InverseProperty("FileConversions")]
    public virtual File File { get; set; } = null!;

    [InverseProperty("FileConversion")]
    public virtual ICollection<FileConversionResult> FileConversionResults { get; set; } = new List<FileConversionResult>();

    [ForeignKey("FileTypeConversionId")]
    [InverseProperty("FileConversions")]
    public virtual FileTypeConversion FileTypeConversion { get; set; } = null!;
}
