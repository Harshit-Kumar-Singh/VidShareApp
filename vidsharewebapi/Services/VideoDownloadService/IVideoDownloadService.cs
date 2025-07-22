using VidShareWebApi.Models;

namespace VidShareWebApi.Services.VideoDownloadService
{
    public interface IVideoDownloadService
    {
        public VideoDownloadUrls GetVideoDownloadUrls(string Key);
    }
}