using Amazon.DynamoDBv2;
using WebApiDynamodbLocal.Services.BigTimeDeals;
using WebApiDynamodbLocal.Services.BigTimeDeals.Interfaces;
using WebApiDynamodbLocal.Services.ECommerce;
using WebApiDynamodbLocal.Services.ECommerce.Interfaces;
using WebApiDynamodbLocal.Services.SessionStore;
using WebApiDynamodbLocal.Services.SessionStore.interfaces;

var builder = WebApplication.CreateBuilder(args);

// Logger
// builder.Logging.ClearProviders().AddJsonConsole();

// CORS設定
var clientUrl = builder.Configuration["ClientUrl"];
builder.Services.AddCors(o => o.AddPolicy(clientUrl, corsPolicyBuilder =>
{
    corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

// DynamoDB
// Console.WriteLine($"DynamoDB Region: {region}");
// Console.WriteLine($"DynamoDB ServiceURL: {amazonDynamoDbConfig.ServiceURL}");
var dynamoDbConfig = builder.Configuration.GetSection("DynamoDb");
builder.Services.AddScoped<AmazonDynamoDBClient>(_ =>
{
    var clientConfig = new AmazonDynamoDBConfig
    {
        ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl")
    };
    return new AmazonDynamoDBClient(clientConfig);
});
// SessionStore
builder.Services.AddScoped<ISessionService, SessionService>();
// ECommerce
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// BigTimeDeals
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IDealService, DealService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// cf. https://stackoverflow.com/questions/69991983/deps-file-missing-for-dotnet-6-integration-tests
// テストを動作させるために追加
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program
{
}
