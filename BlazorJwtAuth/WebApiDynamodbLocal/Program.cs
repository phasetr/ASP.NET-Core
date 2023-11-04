using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using WebApiDynamodbLocal.Services;
using WebApiDynamodbLocal.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Logger
builder.Logging.ClearProviders().AddJsonConsole();

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
var amazonDynamoDbConfig = new AmazonDynamoDBConfig();
// 開発環境だけ`ServiceURL`を`DynamoDB Local`に設定する
// `ServiceURL`は`compose.yml`で設定
if (builder.Environment.IsDevelopment())
{
    amazonDynamoDbConfig.ServiceURL = "http://localhost:8000";
    amazonDynamoDbConfig.RegionEndpoint = RegionEndpoint.GetBySystemName("local");
}
else
{
    var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.USEast2.SystemName;
    amazonDynamoDbConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.GetBySystemName(region)};
}

// デバッグ用
// Console.WriteLine($"DynamoDB Region: {region}");
// Console.WriteLine($"DynamoDB ServiceURL: {amazonDynamoDbConfig.ServiceURL}");
builder.Services
    .AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(amazonDynamoDbConfig))
    .AddScoped<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<AmazonDynamoDBClient>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

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
