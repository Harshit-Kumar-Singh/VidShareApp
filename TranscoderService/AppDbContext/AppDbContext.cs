using Microsoft.EntityFrameworkCore;
using TranscoderService.Models;

namespace TranscoderService.AppDbContextX
{
    public class AppDbContext : DbContext
    {
        public DbSet<VideoDownloadUrls> VideoDownloadUrls { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}