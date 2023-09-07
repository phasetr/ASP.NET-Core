namespace BlazorJwtAuth.Common.Dto;

public class UserGetByEmailResultDto
{
    public string UserId { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
