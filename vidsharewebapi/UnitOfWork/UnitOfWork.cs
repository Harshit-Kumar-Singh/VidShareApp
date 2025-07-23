using VidShareWebApi.Data;
using VidShareWebApi.Repositories.Users;
using VidShareWebApi.Repositories.VideoDownloadUrlsRepo;
using VidShareWebApi.Repositories.VideoInfoRepo;

namespace VidShareWebApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IVideoInfoRepo VideoInfo { get;  private set; }
        public IVideoDownloadUrlsRepo VideoDownload { get; private set; }

        public IUserRepo User { get; private set; }

        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
            VideoInfo = new VideoInfoRepo(context);
            VideoDownload = new VideoDownloadUrlsRepo(context);
            User = new UserRepo(context);
        }

        public async Task<int> SaveChanges()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context.Dispose();
        }

    

    }
}