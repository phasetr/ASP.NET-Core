using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Common.Dto;

public class UserRegisterResultDto : ResponseBase
{
    public bool Succeeded { get; set; }

    public IEnumerable<string> Errors { get; set; } = default!;
}
