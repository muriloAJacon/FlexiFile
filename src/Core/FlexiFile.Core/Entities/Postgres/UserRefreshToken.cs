using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("UserRefreshToken", Schema = "FormatFlex")]
public partial class UserRefreshToken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("created_at", TypeName = "timestamp(3) without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("expires_at", TypeName = "timestamp(3) without time zone")]
    public DateTime ExpiresAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserRefreshTokens")]
    public virtual User User { get; set; } = null!;
}
