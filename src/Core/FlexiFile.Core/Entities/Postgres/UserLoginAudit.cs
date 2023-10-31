using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("UserLoginAudit", Schema = "FlexiFile")]
[Index("UserId", Name = "IX_UserLoginAudit_user_id")]
public partial class UserLoginAudit
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("successful")]
    public bool Successful { get; set; }

    [Column("source_ip", TypeName = "character varying")]
    public string SourceIp { get; set; } = null!;

    [Column("source_user_agent", TypeName = "character varying")]
    public string SourceUserAgent { get; set; } = null!;

    [Column("timestamp")]
    [Precision(3, 0)]
    public DateTime Timestamp { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserLoginAudits")]
    public virtual User User { get; set; } = null!;
}
