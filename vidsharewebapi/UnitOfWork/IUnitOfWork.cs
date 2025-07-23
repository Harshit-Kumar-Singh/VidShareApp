using VidShareWebApi.Repositories.Users;
using VidShareWebApi.Repositories.VideoDownloadUrlsRepo;
using VidShareWebApi.Repositories.VideoInfoRepo;

namespace VidShareWebApi.UnitOfWork
{
    public interface IUnitOfWork
    {
        IVideoInfoRepo VideoInfo { get; }
        IVideoDownloadUrlsRepo VideoDownload { get; }

        IUserRepo  User { get; }
        Task<int> SaveChanges();
        void Dispose();

    }
}