namespace Common.Dto;

public class AuthenticationResponseDto : ResponseBaseDto
{
    public string Email { get; set; } = default!;
    public bool IsAuthenticated { get; set; }
    public string RefreshToken { get; set; } = default!;
    public DateTime RefreshTokenExpiration { get; set; }
    public List<string> Roles { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string UserName { get; set; } = default!;
}
