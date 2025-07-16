using System.Diagnostics;
using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Transfer;
using Confluent.Kafka;
namespace TranscoderService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IAmazonS3 _s3Client;
    private readonly IConsumer<string, string> _consumer;
    private const string BucketName = "vidshare-video-upload-bucket";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);

        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092", // adjust for EC2 or Docker
            GroupId = "transcoder-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe("video_uploaded");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;

                var payload = JsonSerializer.Deserialize<VideoUploadedEvent>(message);
                ProcessVideoAsync(payload!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in processing video.");
            }
        }
    }

    private void ProcessVideoAsync(VideoUploadEvent payload)
    {
        Console.Writeline(payload);
    }
}
