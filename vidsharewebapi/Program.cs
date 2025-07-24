using System.Text;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VidShareWebApi.Data;
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



var builder = WebApplication.CreateBuilder(args);

var connectionstring = "Server=localhost;Port=3306;Database=vidshare;User=root;Password=root;";


// Registering DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionstring,
        new MySqlServerVersion(new Version(8, 0, 6)
    )));

const string jwtKey = "myJwtKeymyJwtKeymyJwtKeymyJwtKey";


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

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return "VidShareBackend running";
});
app.MapControllers();
app.Run();

