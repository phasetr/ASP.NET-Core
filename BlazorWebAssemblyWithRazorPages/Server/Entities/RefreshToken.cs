using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAssemblyWithRazorPages.Server.Entities;

[Owned]
public class RefreshToken
{
    [Key] [JsonIgnore] public int Id { get; set; }

    public string Token { get; set; } = default!;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = default!;
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; } = default!;
    public string ReplacedByToken { get; set; } = default!;
    public string ReasonRevoked { get; set; } = default!;
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}
