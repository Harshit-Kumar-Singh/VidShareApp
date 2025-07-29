using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.DTOs;
using VidShareWebApi.Models;
using VidShareWebApi.Utils;

namespace VidShareWebApi.Services.VideoUploadService
{
    public interface IVideoUploadService
    {
        Task<ServiceResult<VideoInfoDtos>> SaveVideoInfo(VideoInfo info);
        Task<ServiceResult<VideoInfoDtos>> VideoUpload(int userId, IFormFile mediaFile, string uploadId, string title);
        Task<ServiceResult<VideoInfoDtos>> GetDownloadUrls(string keyId);
    }
}