namespace WebApiDynamodbLocal.Dto.SessionStore;

public class GetResponseDto : ResponseBaseDto
{
    public string UserName { get; set; } = default!;
}
