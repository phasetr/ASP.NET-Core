using Humanizer;
using Microsoft.Azure.Cosmos;

namespace App;

public static class CosmosHandler
{
    private static readonly CosmosClient Client;

    static CosmosHandler()
    {
        Client = new CosmosClient(
            "",
            ""
        );
    }

    public static async Task ManageCustomerAsync(string name, string email, string state, string country)
    {
        var container = await GetContainerAsync();
        var id = name.Kebaberize();
        var customer = new
        {
            id,
            name,
            address = new
            {
                state,
                country
            }
        };
        var response = await container.CreateItemAsync(customer);
        Console.WriteLine($"[{response.StatusCode}]\t{id}\t{response.RequestCharge} RUs");
    }

    public static async Task GetCustomerQueryAsync(string name, string email, string state, string country)
    {
        var container = await GetContainerAsync();
        var id = name.Kebaberize();
        var customer = new
        {
            id,
            name,
            address = new
            {
                state,
                country
            }
        };
        // var response = await container.CreateItemAsync(customer);
        string sql = @"SELECT * FROM customers c WHERE c.id = @id";
        var query = new QueryDefinition(query: sql).WithParameter("@id", id);
        using var feed = container.GetItemQueryIterator<dynamic>(queryDefinition: query);
        var response = await feed.ReadNextAsync();
        Console.WriteLine($"[{response.StatusCode}]\t{id}\t{response.RequestCharge} RUs");
    }

    public static async Task ReadCustomerAsync(string name, string email, string state, string country)
    {
        var container = await GetContainerAsync();
        var id = name.Kebaberize();
        var customer = new
        {
            id,
            name,
            address = new
            {
                state,
                country
            }
        };
        var partitionKey = new PartitionKeyBuilder().Add(country).Add(state).Build();
        var response = await container.ReadItemAsync<dynamic>(id: id, partitionKey: partitionKey);
        Console.WriteLine($"[{response.StatusCode}]\t{id}\t{response.RequestCharge} RU");
    }

    public static async Task CreateCustomerCartAsync(string name, string email, string state, string country)
    {
        Container container = await GetContainerAsync();
        string id = name.Kebaberize();
        var customerCart = new
        {
            id = $"{Guid.NewGuid()}",
            customerId = id,
            items = new string[] { },
            address = new
            {
                state = state,
                country = country
            }
        };
        var customerContactInfo = new
        {
            id = $"{id}-contact",
            customerId = id,
            email = email,
            location = $"{state}, {country}",
            address = new
            {
                state = state,
                country = country
            }
        };
        var partitionKey = new PartitionKeyBuilder().Add(country).Add(state).Build();
        var batch = container.CreateTransactionalBatch(partitionKey)
            .ReadItem(id)
            .CreateItem(customerCart)
            .CreateItem(customerContactInfo);
        using var response = await batch.ExecuteAsync();
        Console.WriteLine($"[{response.StatusCode}]\t{response.RequestCharge} RUs");
    }

    private static async Task<Container> GetContainerAsync()
    {
        var database = Client.GetDatabase("cosmicworks");
        List<string> keyPaths = ["/address/country", "/address/state"];
        ContainerProperties properties = new(
            "customers",
            keyPaths
        );
        return await database.CreateContainerIfNotExistsAsync(properties);
    }
}
