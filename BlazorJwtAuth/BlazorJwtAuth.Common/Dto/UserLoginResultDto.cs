using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Common.Dto;

public class UserLoginResultDto : ResponseBase
{
    public bool Succeeded { get; set; }
    public TokenDto Token { get; set; } = default!;
}
