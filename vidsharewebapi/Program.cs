using System.Text;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VidShareWebApi.Data;
using VidShareWebApi.MiddleWares;
using VidShareWebApi.Repositories;
using VidShareWebApi.Repositories.Users;
using VidShareWebApi.Repositories.VideoDownloadUrlsRepo;
using VidShareWebApi.Repositories.VideoInfoRepo;
using VidShareWebApi.Services.AuthService;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.Services.VideoDownloadService;
using VidShareWebApi.Services.VideoUploadService;
using VidShareWebApi.UnitOfWork;
using VidShareWebApi.Utils.S3;
using VidShareWebApi.Utils.SignalR;

using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Threading.Tasks;



var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var connectionstring = config.GetConnectionString("DefaultConnection");
string jwtKey = config["JwtSettings:SecretKey"];



// Registering DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionstring,
        new MySqlServerVersion(new Version(8, 0, 6)
    )));




//Authentication ---- 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.UTF8.GetBytes(jwtKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),
            ValidateIssuer = false,
            ValidateAudience = false


        };
    });


builder.Services.AddSingleton<IAmazonS3>(sp =>
    new AmazonS3Client(Amazon.RegionEndpoint.USEast1)); // adjust region if needed


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"webapi-logs-{DateTime.UtcNow:yyyy.MM}"
    })
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVideoInfoRepo, VideoInfoRepo>();
builder.Services.AddScoped<IVideoUploadService, VideoUploadService>();
builder.Services.AddScoped<IVideoDownloadUrlsRepo, VideoDownloadUrlsRepo>();
builder.Services.AddScoped<IVideoDownloadService, VideoDownloadService>();

builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddSingleton<IKafkaService, KafkaProducerService>();

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IAuthService, AuthService>();



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddSignalR();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.CanConnect())
    {
        System.Console.WriteLine("MySql Database is Connected...");
    }
    else
    {
        Console.WriteLine("Database Not Connected");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(option => option.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseMiddleware<LoggerMiddleWare>();
app.UseMiddleware<ExceptionMiddleware>();



app.MapHub<UploadHub>("/uploadHub");
app.MapGet("/", () =>
{
    Parallel.For(0,10,i=>{
                Console.WriteLine($"Processing {i} on Thread {Thread.CurrentThread.ManagedThreadId}");
            });
    return "VidShareBackend running";
});
app.MapControllers();
app.Run();

