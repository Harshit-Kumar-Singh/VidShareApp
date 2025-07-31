using System.Diagnostics;
using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Transfer;
using Confluent.Kafka;
using Amazon.S3.Model;
using TranscoderService.AppDbContextX;
namespace TranscoderService;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IAmazonS3 _s3Client;
    private readonly IConsumer<string, string> _consumer;
    private  string BucketName = "";

    private readonly IConfiguration configuration;
    private readonly string _tempFolder = Path.Combine(Path.GetTempPath(), "transcoder");

    private readonly IServiceScopeFactory _scopeFactory;
    public Worker(ILogger<Worker> logger,IServiceScopeFactory scopeFactory,IConfiguration _configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
        configuration = configuration;
        BucketName = configuration["S3:BucketName"];
        Directory.CreateDirectory(_tempFolder);

        var config = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = configuration["Kafka:transcoder-group"],
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(config).Build();
        _consumer.Subscribe(configuration["Kafka:Topic"]);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var message = consumeResult.Message.Value;
                System.Console.WriteLine(message);
                // var payload = JsonSerializer.Deserialize<VideoUploadedEvent>(message);
                ProcessVideoAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in processing video.");
            }
        }
    }

    private async Task RunFFmpegAsync(string inputPath, string outputPath, string resolution)
    {
        try
        {


            // Run FFmpeg to convert to given resolution
            string width = "854";
            string height = "480";
            string arguments = $"-i \"{inputPath}\" -vf scale={width}:{height} \"{outputPath}\"";

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/opt/homebrew/bin/ffmpeg",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                string error = await process.StandardError.ReadToEndAsync();
                throw new Exception($"FFmpeg failed for {resolution}p: {error}");
            }

            // Upload converted video back to S3
            string outputKey = $"converted/{Path.GetFileName(outputPath)}";
            // await fileTransferUtility.UploadAsync(outputFile, _bucketName, outputKey);

            // string downloadUrl = $"https://{_bucketName}.s3.amazonaws.com/{outputKey}";
            // outputUrls.Add(downloadUrl);
            


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message.ToString());
        }

    }

    private async void ProcessVideoAsync(string s3Key)
    {

        try
        {
            string inputPath = Path.Combine(_tempFolder, Path.GetFileName(s3Key));
            string output720p = Path.ChangeExtension(inputPath, ".720p.mp4");
            string output480p = Path.ChangeExtension(inputPath, ".480p.mp4");

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.DownloadAsync(inputPath, BucketName, s3Key);
            Console.WriteLine($"Downloaded {s3Key} to {inputPath}");


            //await RunFFmpegAsync(inputPath, output720p, "1280x720");


            //Here will implement Task thing to to thing in parallelly

            await RunFFmpegAsync(inputPath, output480p, "854:480");
            System.Console.WriteLine("Converted into resoltution");
            // 3. Upload transcoded videos to S3
            string outputKey720p = $"transcoded/720p/{Path.GetFileName(output720p)}";
            string outputKey480p = $"transcoded/480p/{Path.GetFileName(output480p)}";


            
            //await fileTransferUtility.UploadAsync(output720p, _bucketName, outputKey720p);
            await fileTransferUtility.UploadAsync(output480p, BucketName, outputKey480p);

            var request = new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = outputKey480p,
                Expires = DateTime.UtcNow.AddHours(1)
            };

            string downloadUrl = _s3Client.GetPreSignedURL(request);

            System.Console.WriteLine("Uploaded...");

            System.Console.WriteLine(downloadUrl); ;


            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var downloadUrlsToUpdate = dbContext.VideoDownloadUrls.Where(_x => _x.KeyId == s3Key).First();
            downloadUrlsToUpdate.DownloadUrl480 = downloadUrl;
            await dbContext.SaveChangesAsync();
            Console.WriteLine("Updated in Db");


        }
        catch (System.Exception)
        {
            System.Console.WriteLine("Error");
        }

    }
}
