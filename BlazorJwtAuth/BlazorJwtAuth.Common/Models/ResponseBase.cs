namespace BlazorJwtAuth.Common.Models;

public class ResponseBase
{
    public string Detail { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Status { get; set; } = default!;
}
