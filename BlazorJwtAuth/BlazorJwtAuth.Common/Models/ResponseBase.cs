namespace BlazorJwtAuth.Common.Models;

public class ResponseBase
{
    public string Status { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Detail { get; set; } = default!;
}
