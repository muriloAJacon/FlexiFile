using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FlexiFile.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace FlexiFile.Core.Entities.Postgres;

[Table("User", Schema = "FlexiFile")]
public partial class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name", TypeName = "character varying")]
    public string Name { get; set; } = null!;

    [Column("email", TypeName = "character varying")]
    public string Email { get; set; } = null!;

    [Column("password", TypeName = "character varying")]
    public string Password { get; set; } = null!;

    [Column("access_level")]
    public AccessLevel AccessLevel { get; set; }

    [Column("approved")]
    public bool Approved { get; set; }

    [Column("approved_at", TypeName = "timestamp(3) with time zone")]
    public DateTime? ApprovedAt { get; set; }

    [Column("approved_by_user_id")]
    public Guid? ApprovedByUserId { get; set; }

    [Column("creation_date", TypeName = "timestamp(3) with time zone")]
    public DateTime CreationDate { get; set; }

    [Column("created_by_user_id")]
    public Guid? CreatedByUserId { get; set; }

    [Column("last_update_date", TypeName = "timestamp(3) with time zone")]
    public DateTime LastUpdateDate { get; set; }

    [Column("storage_limit")]
    public long? StorageLimit { get; set; }

    [ForeignKey("ApprovedByUserId")]
    [InverseProperty("InverseApprovedByUser")]
    public virtual User? ApprovedByUser { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("InverseCreatedByUser")]
    public virtual User? CreatedByUser { get; set; }

    [InverseProperty("OwnedByUser")]
    public virtual ICollection<File> Files { get; set; } = new List<File>();

    [InverseProperty("ApprovedByUser")]
    public virtual ICollection<User> InverseApprovedByUser { get; set; } = new List<User>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<User> InverseCreatedByUser { get; set; } = new List<User>();

    [InverseProperty("UpdatedByUser")]
    public virtual ICollection<Setting> Settings { get; set; } = new List<Setting>();

    [InverseProperty("User")]
    public virtual ICollection<UserLoginAudit> UserLoginAudits { get; set; } = new List<UserLoginAudit>();

    [InverseProperty("User")]
    public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new List<UserRefreshToken>();
}
