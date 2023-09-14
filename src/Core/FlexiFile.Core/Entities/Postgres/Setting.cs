using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("Setting", Schema = "FormatFlex")]
public partial class Setting
{
    [Key]
    [Column("id", TypeName = "character varying")]
    public string Id { get; set; } = null!;

    [Column("value", TypeName = "character varying")]
    public string Value { get; set; } = null!;

    [Column("last_update_date", TypeName = "timestamp(3) without time zone")]
    public DateTime? LastUpdateDate { get; set; }

    [Column("updated_by_user_id")]
    public Guid? UpdatedByUserId { get; set; }

    [ForeignKey("UpdatedByUserId")]
    [InverseProperty("Settings")]
    public virtual User? UpdatedByUser { get; set; }
}
