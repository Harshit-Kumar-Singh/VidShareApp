using VidShareWebApi.Repositories.VideoDownloadUrlsRepo;
using VidShareWebApi.Repositories.VideoInfoRepo;

namespace VidShareWebApi.UnitOfWork
{
    public interface IUnitOfWork
    {
        IVideoInfoRepo VideoInfo { get; }
        IVideoDownloadUrlsRepo VideoDownload { get; }
        Task<int> SaveChanges();
        void Dispose();

    }
}