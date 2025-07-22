using System.Net.Http.Headers;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.Models;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.UnitOfWork;
using VidShareWebApi.Utils;
using VidShareWebApi.Utils.S3;

namespace VidShareWebApi.Services.VideoUploadService
{
    public class VideoUploadService : IVideoUploadService
    {
        private readonly IS3Service _s3Service;
        private readonly IUnitOfWork unitOfWork;

        private readonly IKafkaService kafkaService;
        public VideoUploadService(IS3Service s3Service, IUnitOfWork _unitOfWork, IKafkaService _ks)
        {
            _s3Service = s3Service;
            unitOfWork = _unitOfWork;
            kafkaService = _ks;
        }


        public async Task<ServiceResult<string>> SaveVideoInfo(VideoInfo info)
        {

            try
            {
                info.KeyId = Guid.NewGuid().ToString();


                const string bucketName = "vidshare-video-upload-bucket";
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


                info.VideoDownloadUrls = new List<VideoDownloadUrls> { videoDownloadUrls };
                Console.WriteLine(info);
                await unitOfWork.VideoInfo.SaveInfo(info);

                await unitOfWork.SaveChanges();
                return new ServiceResult<string>
                {
                    Message = "Video Infor Saved Successfully!",
                    Success = true,
                    Result = info.KeyId
                };

            }
            catch (Exception ex)
            {
                // Specific to DynamoDB errors (e.g., table not found, permission denied)
                //return StatusCode(500, new { error = "DynamoDB error", details = ex.Message });
                return new ServiceResult<string>
                {
                    Message = "Error Occurred",
                    Success = false,
                };
            }


        }

        public async Task<ServiceResult<bool>> VideoUpload(string id, IFormFile mediaFile)
        {
            try
            {
                var info = unitOfWork.VideoDownload.GetVideoDownloadUrlsAsync(id);
                var httpClient = new HttpClient();
                var streamContent = new StreamContent(mediaFile.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaFile.ContentType);
                var response = await httpClient.PutAsync(info.PreSignedUrl, streamContent);

                // connect to kafka and send the meta info in it for transcoder servi ce
                const string KAFKA_TOPIC = "test-topic";
                await kafkaService.SendMessageAsync(KAFKA_TOPIC, info.KeyId);

                if (response.IsSuccessStatusCode)
                {
                    return new ServiceResult<bool>
                    {
                        Result = true,
                        Success = true,
                        Message = "Video Uploaded Successfully!"
                    };
                }
                return new ServiceResult<bool>
                {
                    Result = false,
                    Success = false,
                    Message = "Video Uploading failed!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ",");
                 return new ServiceResult<bool>
                 {
                        Result = false,
                        Success = false,
                        Message = ex.Message
                };
            }
            
            
        }
    }
}