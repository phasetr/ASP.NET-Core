namespace BlazorJwtAuth.Common.Dto;

public class ResponseBaseDto
{
    public string Detail { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string Status { get; set; } = default!;
}
