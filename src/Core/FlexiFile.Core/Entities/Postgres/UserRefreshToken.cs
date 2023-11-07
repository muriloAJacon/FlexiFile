using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("UserRefreshToken", Schema = "FlexiFile")]
[Index("UserId", Name = "IX_UserRefreshToken_user_id")]
public partial class UserRefreshToken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("created_at")]
    [Precision(3, 0)]
    public DateTime CreatedAt { get; set; }

    [Column("expires_at")]
    [Precision(3, 0)]
    public DateTime ExpiresAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserRefreshTokens")]
    public virtual User User { get; set; } = null!;
}
