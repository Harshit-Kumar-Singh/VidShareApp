using System.Net.Http.Headers;
using Confluent.Kafka;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VidShareWebApi.DTOs;
using VidShareWebApi.Models;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.UnitOfWork;
using VidShareWebApi.Utils;
using VidShareWebApi.Utils.S3;
using VidShareWebApi.Utils.SignalR;

namespace VidShareWebApi.Services.VideoUploadService
{
    
    
    public class VideoUploadService : IVideoUploadService
    {
        private readonly IS3Service _s3Service;
        private readonly IUnitOfWork unitOfWork;

        private readonly IKafkaService kafkaService;

        private readonly IHubContext<UploadHub> _hubContext;

        IConfiguration configuration;

        public VideoUploadService(IS3Service s3Service, IUnitOfWork _unitOfWork, IKafkaService _ks, IHubContext<UploadHub> hubContext, IConfiguration _configuration)
        {
            _s3Service = s3Service;
            unitOfWork = _unitOfWork;
            kafkaService = _ks;
            _hubContext = hubContext;
            configuration = _configuration;
        }

        public async Task<ServiceResult<VideoInfoDtos>> GetDownloadUrls(string keyId)
        {
            var downloadUrls = unitOfWork.VideoDownload.GetVideoDownloadUrlsAsync(keyId);
            if (string.IsNullOrEmpty(downloadUrls.DownloadUrl480))
            {
                return new ServiceResult<VideoInfoDtos>
                {
                    Success = false,
                    Message = "Not yet uploaded"
                };
            }
            var videoInfo = new VideoInfoDtos
            {
                DownloadUrlRaw = downloadUrls.DownloadUrlRaw,
                DownloadUrl480 = downloadUrls.DownloadUrl480
            };
            return new ServiceResult<VideoInfoDtos>
            {
                Success = true,
                Message = "Success",
                Result = videoInfo
            };
        }

        public async Task<ServiceResult<VideoInfoDtos>> SaveVideoInfo(VideoInfo info)
        {

            try
            {
                info.KeyId = Guid.NewGuid().ToString();


                 string bucketName = configuration["S3:BucketName"];
                // Generate pre-signed URL
                string preSignedUrl = _s3Service.GetPreSignedUploadUrl(
                    bucketName: bucketName,
                    objectKey: info.KeyId,
                    expiry: TimeSpan.FromMinutes(15)
                );

                string _downloadUrl = _s3Service.GetDownloadUrl(
                    bucketName: bucketName,
                    objectKey: info.KeyId,
                    expiry: TimeSpan.FromMinutes(60)
                );

                VideoDownloadUrls videoDownloadUrls = new VideoDownloadUrls
                {
                    KeyId = info.KeyId,
                    PreSignedUrl = preSignedUrl,
                    DownloadUrlRaw = _downloadUrl,
                };

                var videoInfoDto = new VideoInfoDtos
                {
                    KeyId = info.KeyId,
                    PreSignedUrl = preSignedUrl,
                    DownloadUrlRaw = _downloadUrl
                };


                info.VideoDownloadUrls = new List<VideoDownloadUrls> { videoDownloadUrls };
                Console.WriteLine(info);
                await unitOfWork.VideoInfo.SaveInfo(info);

                await unitOfWork.SaveChanges();
                return new ServiceResult<VideoInfoDtos>
                {
                    Message = "Video Infor Saved Successfully!",
                    Success = true,
                    Result = videoInfoDto
                };

            }
            catch (Exception ex)
            {
                // Specific to DynamoDB errors (e.g., table not found, permission denied)
                //return StatusCode(500, new { error = "DynamoDB error", details = ex.Message });
                return new ServiceResult<VideoInfoDtos>
                {
                    Message = "Error Occurred",
                    Success = false,
                };
            }


        }

        public async Task<ServiceResult<VideoInfoDtos>> VideoUpload(int userId,IFormFile mediaFile, string uploadId, string title)
        {
            try
            {
                VideoInfo videoInfo = new VideoInfo
                {
                    Title = title,
                    UserId = userId
                };
                ServiceResult<VideoInfoDtos> res = await SaveVideoInfo(videoInfo);
                var info = res.Result;
                string id = res.Result.KeyId;
                Console.WriteLine(id);

                /*
                // SignalR;
                var fileStream =  mediaFile.OpenReadStream();
                long totalBytes = fileStream.Length;
                byte[] buffer = new byte[81920]; // 80KB chunks
                int bytesRead;
                long uploadedBytes = 0;

                using var s3Stream = new MemoryStream(); // or directly stream to S3
                while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await s3Stream.WriteAsync(buffer, 0, bytesRead);
                    uploadedBytes += bytesRead;

                    int percent = (int)((uploadedBytes * 100) / totalBytes);
                    Console.WriteLine(percent + '%');
                    await _hubContext.Clients.Group(uploadId).SendAsync("ReceiveProgress", percent);
                }




                var streamContent = new StreamContent(mediaFile.OpenReadStream());

                var httpClient = new HttpClient();
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaFile.ContentType);
                var response = await httpClient.PutAsync(info.PreSignedUrl, streamContent);
                */

                var fileStream = mediaFile.OpenReadStream();
                var progressContent = new ProgressableStreamContent(
                    fileStream,
                    81920,
                    uploadedBytes =>
                    {
                        int percent = (int)((uploadedBytes * 100) / fileStream.Length);
                        Console.WriteLine($"{percent}%");
                        _ = _hubContext.Clients.Group(uploadId).SendAsync("ReceiveProgress", percent);
                    },
                    fileStream.Length
                );
                progressContent.Headers.ContentType = new MediaTypeHeaderValue(mediaFile.ContentType);

                var httpClient = new HttpClient();
                var response = await httpClient.PutAsync(info.PreSignedUrl, progressContent);




                Console.WriteLine(response.IsSuccessStatusCode);
                // connect to kafka and send the meta info in it for transcoder servi ce
                 string KAFKA_TOPIC = configuration["Kafka:Topic"];
                await kafkaService.SendMessageAsync(KAFKA_TOPIC, info.KeyId);

            
                 return new ServiceResult<VideoInfoDtos>
                    {
                        Result = info,
                        Success = true,
                        Message = "Video Uploaded Successfully!"
                    };
                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ",");
                return new ServiceResult<VideoInfoDtos>
                {
                
                    Success = false,
                    Message = ex.Message
                };
            }


        }
    }
}