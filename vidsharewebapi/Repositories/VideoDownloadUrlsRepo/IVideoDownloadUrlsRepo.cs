using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories.VideoDownloadUrlsRepo
{
    public interface IVideoDownloadUrlsRepo
    {
        Task<bool> SaveDownloadUrls(VideoDownloadUrls item);
        VideoDownloadUrls GetVideoDownloadUrlsAsync(string Key);
    }    
}
