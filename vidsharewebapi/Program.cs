using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Microsoft.AspNetCore.Http.HttpResults;
using VidShareWebApi.Repositories;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.Utils.S3;

var builder = WebApplication.CreateBuilder(args);


// Add DynamoDB
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
    new AmazonDynamoDBClient(Amazon.RegionEndpoint.USEast1)); // use your region

builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();

builder.Services.AddSingleton<IAmazonS3>(sp =>
    new AmazonS3Client(Amazon.RegionEndpoint.USEast1)); // adjust region if needed

builder.Services.AddScoped<IS3Service, S3Service>();
builder.Services.AddSingleton<IKafkaService, KafkaProducerService>();


// Add your repository
builder.Services.AddScoped<IVideoRepository, VideoRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

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

