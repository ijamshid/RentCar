using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Minio;
using Newtonsoft.Json;
using Rentcar.Application.Services.Implementation;
using RentCar.API.Middlewares;
using RentCar.Application;
using RentCar.Application.Common;
using RentCar.Application.Helpers;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Application.Security.AuthEnums;
using RentCar.Application.Services;
using RentCar.DataAccess;
using RentCar.DataAccess.Persistence;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);

//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.Configure<EmailConfiguration>(
    builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.Configure<JwtOption>(
    builder.Configuration.GetSection("JwtOption"));
// Add services to the container.

var config = builder.Configuration;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddDataAccess(builder.Configuration).AddApplication().AddAuth(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    // ApplicationPermissionCode enumining barcha qiymatlari bo'yicha policy yaratish
    foreach (var permissionName in Enum.GetNames(typeof(ApplicationPermissionCode)))
    {
        options.AddPolicy(permissionName, policy =>
        policy.RequireClaim("permission", permissionName));
    }
});

//builder.Services.AddControllers()
//    .AddNewtonsoftJson(options =>
//    {
//        options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
//    });

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );


builder.Services.AddSingleton<IFileStorageService, MinioFileStorageService>();
builder.Services.Configure<MinioSettings>(config.GetSection("MinioSettings"));

builder.Services.AddSingleton<IMinioClient>(sp =>
{
    var minioSettings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

    // MinioClient obyektini yaratish
    var client = new MinioClient()
      .WithEndpoint(minioSettings.Endpoint)
      .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey);

    // Agar SSL yoqilgan bo'lsa
    if (minioSettings.UseSsl)
    {
        client = client.WithSSL();
    }

    return client.Build(); // MinioClient ni qurish
});


builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("default", builder =>
    {
        builder.Window = TimeSpan.FromMinutes(1);
        builder.PermitLimit = 5;
        builder.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        builder.QueueLimit = 2; // Allow up to 10 requests in the queue
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(8080); // HTTPS kerak emas
//});





// Configure the HTTP request pipeline.
//if (builder.Environment.IsProduction() && builder.Configuration.GetValue<int?>("PORT") is not null)
  //
  //builder.WebHost.UseUrls($"https://*:{builder.Configuration.GetValue<int>("Port")}");
var app = builder.Build();
// Migratsiya
using (var scope = app.Services.CreateScope())
{
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await AutomatedMigration.MigrateAsync(scope.ServiceProvider);

    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Migration failed.");
    }
}


app.UseSwagger();
app.UseSwaggerUI();

// end

//app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

app.Run();
