using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("FileConversionResult", Schema = "FlexiFile")]
[Index("FileConversionId", Name = "IX_FileConversionResult_file_conversion_id")]
public partial class FileConversionResult
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

	[Column("type_id")]
	public int TypeId { get; set; }

	[Column("file_conversion_id")]
    public Guid FileConversionId { get; set; }

    [Column("creation_date")]
    [Precision(3, 0)]
    public DateTime CreationDate { get; set; }

	[Column("size")]
	public long Size { get; set; }

	[Column("order")]
    public int Order { get; set; }

    [ForeignKey("FileConversionId")]
    [InverseProperty("FileConversionResults")]
    public virtual FileConversion FileConversion { get; set; } = null!;

	[ForeignKey("TypeId")]
	[InverseProperty("FileConversionResults")]
	public virtual FileType Type { get; set; } = null!;
}
