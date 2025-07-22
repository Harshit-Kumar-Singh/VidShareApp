using VidShareWebApi.Models;
using VidShareWebApi.UnitOfWork;
using VidShareWebApi.Utils;

namespace VidShareWebApi.Services.VideoDownloadService
{
    public class VideoDownloadService : IVideoDownloadService
    {
        private readonly IUnitOfWork unitOfWork;
        public VideoDownloadService(IUnitOfWork _uow)
        {
            unitOfWork = _uow;
        }

        public VideoDownloadUrls GetVideoDownloadUrls(string Key)
        {
            var data = unitOfWork.VideoDownload.GetVideoDownloadUrlsAsync(Key);
            return data;
        }
    }

}