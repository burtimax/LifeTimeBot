using System.Reflection;
using FastEndpoints;
using FastEndpoints.Swagger;
using LifeTimeBot.App.Options;
using LifeTimeBot.Db.AppDb;
using LifeTimeBot.Extensions;
using LifeTimeBot.Resources;
using Microsoft.EntityFrameworkCore;
using MultipleBotFramework.Extensions;
using MultipleBotFramework.Models;
using MultipleBotFramework.Options;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddFastEndpoints(o =>
    {
        o.Assemblies = new[]
        {
            typeof(GetBotsEndpoint).Assembly,
            typeof(Program).Assembly,
        };
    })
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Multiple Bots Template Project API";
            s.Version = "v1";
        };
    });


var services = builder.Services;
services.AddServices();
services.Configure<AppOptions>(builder.Configuration);
var config = builder.Configuration.Get<AppOptions>();


// Добавляем контексты
services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(config.DbConnections.AppDbConnection,
            sqlOptions => sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        options.EnableSensitiveDataLogging();
    }
);

// Регистрируем конфигурации.
services.Configure<BotConfiguration>(builder.Configuration.GetSection("Bot"));
services.Configure<BotOptions>(builder.Configuration.GetSection("BotOptions"));
var botConfig = builder.Configuration.GetSection("Bot").Get<BotConfiguration>();
BotOptions botOptions = builder.Configuration.GetSection("BotOptions").Get<BotOptions>();
BotResources botResources = services.ConfigureBotResources(botConfig.ResourcesFilePath);
services.AddBot(botConfig, botOptions: botOptions); // Подключаем бота
services.AddControllers();//.AddNewtonsoftJson(); //Обязательно подключаем NewtonsoftJson
services.AddHttpContextAccessor();

bool CorsConfigIsNotNull() =>
    config.Cors is not null && config.Cors.AllowOrigins is not null && config.Cors.AllowOrigins.Any();

if (CorsConfigIsNotNull())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: "AllowOnlySomeOrigins",
            configurePolicy: policy =>
            {
                policy.WithOrigins(config.Cors.AllowOrigins.ToArray());
            });
    });
}
else
{
    builder.Services.AddCors();
}

services.AddMapster();

// Свои сервисы


// Регистрируем контексты к базам данных.

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Провести миграцию в БД.
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseFastEndpoints().UseSwaggerGen();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
//app.UseHttpsRedirection();

if (CorsConfigIsNotNull())
{
    app.UseCors("AllowOnlySomeOrigins");
}
else
{
    app.UseCors(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();