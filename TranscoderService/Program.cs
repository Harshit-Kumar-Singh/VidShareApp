using Microsoft.EntityFrameworkCore;
using TranscoderService;
using TranscoderService.AppDbContextX;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

const string connectionString = "Server=localhost;Port=3306;Database=vidshare;User=root;Password=root;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 6))
    ));
var host = builder.Build();
host.Run();
