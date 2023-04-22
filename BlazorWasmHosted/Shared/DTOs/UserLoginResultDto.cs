namespace BlazorWasmHosted.Shared.DTOs;

public class UserLoginResultDto
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = default!;
    public TokenDto Token { get; set; } = default!; 
}
