namespace Common.Dto;

public class UserLoginResponseDto : ResponseBaseDto
{
    public TokenDto Token { get; set; } = default!;
}
