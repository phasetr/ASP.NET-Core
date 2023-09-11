namespace BlazorJwtAuth.Common.Dto;

public class UserRegisterResponseDto : ResponseBaseDto
{
    public bool Succeeded { get; set; }

    public IEnumerable<string> Errors { get; set; } = default!;
}
