namespace BlazorWasmHosted.Shared.DTOs;

public class UserRegisterResultDto
{
    public bool Succeeded { get; set; }

    public IEnumerable<string> Errors { get; set; } = default!;
}
