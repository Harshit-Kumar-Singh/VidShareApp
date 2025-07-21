using VidShareWebApi.Data;
using VidShareWebApi.Repositories.VideoInfoRepo;

namespace VidShareWebApi.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public IVideoInfoRepo VideoInfo { get;  private set; }

        public UnitOfWork(AppDbContext _context)
        {
            context = _context;
            VideoInfo = new VideoInfoRepo(context);
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