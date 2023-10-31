using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("FileTypeConversion", Schema = "FlexiFile")]
[Index("FromTypeId", Name = "IX_FileTypeConversion_from_type_id")]
[Index("ToTypeId", Name = "IX_FileTypeConversion_to_type_id")]
public partial class FileTypeConversion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// (&quot;Conversion&quot;,&quot;Processing&quot;)
    /// </summary>
    [Column("type")]
    public string Type { get; set; } = null!;

    [Column("from_type_id")]
    public int FromTypeId { get; set; }

    [Column("to_type_id")]
    public int? ToTypeId { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("handler_class_name", TypeName = "character varying")]
    public string HandlerClassName { get; set; } = null!;

    [InverseProperty("FileTypeConversion")]
    public virtual ICollection<FileConversion> FileConversions { get; set; } = new List<FileConversion>();

    [ForeignKey("FromTypeId")]
    [InverseProperty("FileTypeConversionFromTypes")]
    public virtual FileType FromType { get; set; } = null!;

    [ForeignKey("ToTypeId")]
    [InverseProperty("FileTypeConversionToTypes")]
    public virtual FileType? ToType { get; set; }
}
