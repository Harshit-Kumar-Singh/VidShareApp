using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.Models;
using VidShareWebApi.UnitOfWork;
using VidShareWebApi.Utils;
using VidShareWebApi.Utils.S3;

namespace VidShareWebApi.Services.VideoUploadService
{
    public class VideoUploadService : IVideoUploadService
    {
        private readonly IS3Service _s3Service;
        private readonly IUnitOfWork unitOfWork;
        VideoUploadService(IS3Service s3Service, IUnitOfWork _unitOfWork)
        {
            _s3Service = s3Service;
            unitOfWork = _unitOfWork;
        }


        public async Task<ServiceResult<string>> SaveVideoInfo(VideoInfo info)
        {

            try
            {
                info.KeyId = Guid.NewGuid().ToString();
                await unitOfWork.VideoInfo.SaveInfo(info);

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
                    DownloadUrlRaw = _downloadUrl
                };


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
    }
}