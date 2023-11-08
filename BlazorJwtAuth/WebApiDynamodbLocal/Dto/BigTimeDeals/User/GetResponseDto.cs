using WebApiDynamodbLocal.Models.BigTimeDeals;

namespace WebApiDynamodbLocal.Dto.BigTimeDeals.User;

public class GetResponseDto : ResponseBaseDto
{
    public UserModel? UserModel { get; set; }
}
