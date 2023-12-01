using Amazon.DynamoDBv2;
using AspNetCore.Identity.AmazonDynamoDB;
using Cdk.Common;
using Microsoft.Extensions.DependencyInjection;

const string serviceUrl = Constants.DynamoDbLocalUrl;

var services = new ServiceCollection();
var client = new AmazonDynamoDBClient(
    string.IsNullOrEmpty(serviceUrl) == false
        ? new AmazonDynamoDBConfig {ServiceURL = serviceUrl}
        : new AmazonDynamoDBConfig());

services.AddIdentityCore<DynamoDbUser>()
    .AddRoles<DynamoDbRole>()
    .AddDynamoDbStores().Configure(options => { options.DefaultTableName = "my-custom-identity-table-name"; });
services.AddSingleton<IAmazonDynamoDB>(client);

var serviceProvider = services.BuildServiceProvider();
// Ensure Tables Is Created
AspNetCoreIdentityDynamoDbSetup.EnsureInitialized(serviceProvider);
var tables = client.ListTablesAsync().GetAwaiter().GetResult();
Console.WriteLine("Tables initialized, the following tables exists:");
tables.TableNames.ForEach(Console.WriteLine);

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("All good!");
Console.ResetColor();
