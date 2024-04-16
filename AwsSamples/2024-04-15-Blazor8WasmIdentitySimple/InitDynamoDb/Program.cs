using Amazon;
using Amazon.DynamoDBv2;
using AspNetCore.Identity.AmazonDynamoDB;
using Common;
using Microsoft.Extensions.DependencyInjection;

InitializeDynamoDb(MyEnvironment.Local);
// InitializeDynamoDb(MyEnvironment.Dev);
return;

void InitializeDynamoDb(MyEnvironment env)
{
    var services = new ServiceCollection();
    var amazonDynamoDbConfig = env switch
    {
        MyEnvironment.Local => new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.APNortheast1,
            ServiceURL = Constants.DynamoDbLocalUrl
        },
        MyEnvironment.Dev => new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.APNortheast1
        },
        _ => throw new ArgumentOutOfRangeException(nameof(env), env, null)
    };
    var client = new AmazonDynamoDBClient(amazonDynamoDbConfig);

    services.AddIdentityCore<DynamoDbUser>()
        .AddRoles<DynamoDbRole>()
        .AddDynamoDbStores().Configure(options =>
        {
            options.DefaultTableName = env switch
            {
                MyEnvironment.Local => Constants.DynamoDbLocalTableName,
                MyEnvironment.Dev => Constants.DynamoDbDevTableName,
                _ => throw new ArgumentOutOfRangeException(nameof(env), env, null)
            };
        });
    services.AddSingleton<IAmazonDynamoDB>(client);

    var serviceProvider = services.BuildServiceProvider();
    // Ensure Tables Is Created
    AspNetCoreIdentityDynamoDbSetup.EnsureInitialized(serviceProvider);
    var tables = client.ListTablesAsync().GetAwaiter().GetResult();
    Console.WriteLine($"NOW {env}");
    Console.WriteLine("Tables initialized, the following tables exists:");
    tables.TableNames.ForEach(Console.WriteLine);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("All good!");
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine();
}
