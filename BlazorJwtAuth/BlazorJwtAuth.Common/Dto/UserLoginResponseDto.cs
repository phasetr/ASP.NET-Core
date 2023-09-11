namespace BlazorJwtAuth.Common.Dto;

public class UserLoginResponseDto : ResponseBaseDto
{
    public bool Succeeded { get; set; }
    public TokenDto Token { get; set; } = default!;
}
