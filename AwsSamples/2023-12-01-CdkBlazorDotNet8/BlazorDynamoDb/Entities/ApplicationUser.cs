using AspNetCore.Identity.AmazonDynamoDB;

namespace BlazorDynamoDb.Entities;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : DynamoDbUser
{
}
