namespace BlazorWasmHosted.Server.Models.Authentication;

public class CreateTokenData
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
