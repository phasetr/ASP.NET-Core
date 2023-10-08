using System.ComponentModel.DataAnnotations;

namespace Common.EntityModels.Entities;

public class RefreshToken
{
    [Key] public string Token { get; set; } = default!;
    public string ApplicationUserId { get; set; } = default!;
    public ApplicationUser ApplicationUser { get; set; } = default!;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsActive => Revoked == null && !IsExpired;
}
