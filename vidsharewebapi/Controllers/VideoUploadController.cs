using System.Net.Http.Headers;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.DTOs;
using VidShareWebApi.Models;
using VidShareWebApi.Repositories;
using VidShareWebApi.Repositories.VideoDownloadUrlsRepo;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.Services.VideoDownloadService;
using VidShareWebApi.Services.VideoUploadService;
using VidShareWebApi.Utils;
using VidShareWebApi.Utils.S3;

namespace VidShareWebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api")]
    public class VideoUploadController : ControllerBase
    {
        private readonly IVideoUploadService videoUploadService;
        private readonly IVideoDownloadService videoDownloadService;
        private readonly IKafkaService kafkaService;
        public VideoUploadController(IVideoUploadService _videoUploadService, IKafkaService _kafkaService, IVideoDownloadService _videoDownloadService)
        {
            videoUploadService = _videoUploadService;
            videoDownloadService = _videoDownloadService;

        }

        [HttpPut]
        [Route("save-video-info")]
        public async Task<ServiceResult<VideoInfoDtos>> SaveVideoInfo([FromBody] VideoInfo info)
        {

            return await videoUploadService.SaveVideoInfo(info);

        }

        [HttpPost]
        [Route("upload-video")]
        public async Task<ServiceResult<VideoInfoDtos>> UploadVideo([FromForm] IFormFile mediaFile, [FromForm] string uploadId, [FromForm] string title)
        {
            int userId = 0;
            try
            {
                Int32.TryParse(User.FindFirst("UserId").Value.ToString(), out userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ServiceResult<VideoInfoDtos>
                {
                    Message = "Error",
                    Success = false
                };
            }


            return await videoUploadService.VideoUpload(userId, mediaFile, uploadId, title);
        }


        [HttpGet]
        [Route("get-download-urls/{keyId}")]
        public async Task<ServiceResult<VideoInfoDtos>> GetDownloadUrls([FromRoute]string keyId)
        {
            return await videoUploadService.GetDownloadUrls(keyId);
        }
    }
}