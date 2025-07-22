using VidShareWebApi.Data;
using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories.VideoDownloadUrlsRepo
{
    public class VideoDownloadUrlsRepo : IVideoDownloadUrlsRepo
    {
        private readonly AppDbContext context;
        public VideoDownloadUrlsRepo(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<bool> SaveDownloadUrls(VideoDownloadUrls item)
        {
            try
            {
                await context.VideoDownloadUrls.AddAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public VideoDownloadUrls GetVideoDownloadUrlsAsync(string Key)
        {
            var data = context.VideoDownloadUrls.Where(_val => _val.KeyId == Key).FirstOrDefault();
            return data;
        }
    }
}