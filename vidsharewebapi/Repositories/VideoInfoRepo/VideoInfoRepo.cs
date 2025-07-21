
using VidShareWebApi.Data;
using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories.VideoInfoRepo
{
    public class VideoInfoRepo : IVideoInfoRepo
    {
        private readonly AppDbContext context;
        public VideoInfoRepo(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<bool> SaveInfo(VideoInfo videoInfo)
        {
            try
            {
                await context.VideoInfos.AddAsync(videoInfo);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }
            
            
        }
        
    }
}