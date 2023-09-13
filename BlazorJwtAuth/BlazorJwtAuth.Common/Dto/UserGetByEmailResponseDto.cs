namespace BlazorJwtAuth.Common.Dto;

public class UserGetByEmailResponseDto
{
    public string UserId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
}
