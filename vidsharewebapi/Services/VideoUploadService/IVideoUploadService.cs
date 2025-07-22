using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.Models;
using VidShareWebApi.Utils;

namespace VidShareWebApi.Services.VideoUploadService
{
    public interface IVideoUploadService
    {
        Task<ServiceResult<string>> SaveVideoInfo(VideoInfo info);
        Task<ServiceResult<bool>> VideoUpload(string id, IFormFile mediaFile);
    }
}