using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("FileConversionResult", Schema = "FlexiFile")]
public partial class FileConversionResult
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("file_conversion_id")]
    public Guid FileConversionId { get; set; }

    [Column("creation_date", TypeName = "timestamp(3) without time zone")]
    public DateTime CreationDate { get; set; }

    [Column("order")]
    public int Order { get; set; }

    [ForeignKey("FileConversionId")]
    [InverseProperty("FileConversionResults")]
    public virtual FileConversion FileConversion { get; set; } = null!;
}
