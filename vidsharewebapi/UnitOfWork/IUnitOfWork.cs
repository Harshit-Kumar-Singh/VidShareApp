using VidShareWebApi.Repositories.VideoInfoRepo;

namespace VidShareWebApi.UnitOfWork
{
    public interface IUnitOfWork
    {
        IVideoInfoRepo VideoInfo { get; }
        Task<int> SaveChanges();
        void Dispose();

    }
}