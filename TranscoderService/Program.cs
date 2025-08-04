using Microsoft.EntityFrameworkCore;
using TranscoderService;
using TranscoderService.AppDbContextX;
using System;
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var config = builder.Configuration;
var connectionstring = config.GetConnectionString("DefaultConnection");
Console.WriteLine(connectionstring);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionstring,
        new MySqlServerVersion(new Version(8, 0, 6))
    ));
var host = builder.Build();
host.Run();
