var builder = WebApplication.CreateBuilder(args);

// CORS設定
var clientUrl = builder.Configuration["ClientUrl"];
builder.Services.AddCors(o => o.AddPolicy(clientUrl, corsPolicyBuilder =>
{
    corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors(clientUrl);
app.UseAuthorization();
app.MapControllers();

app.Run();
