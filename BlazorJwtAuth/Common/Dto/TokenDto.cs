namespace Common.Dto;

public class TokenDto
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
