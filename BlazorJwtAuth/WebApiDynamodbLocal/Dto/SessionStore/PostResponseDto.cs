using Common.Dto;

namespace WebApiDynamodbLocal.Dto.SessionStore;

public class PostResponseDto : ResponseBaseDto
{
    public string SessionId { get; set; } = default!;
}
