using Microsoft.EntityFrameworkCore;
using VidShareWebApi.Models;

namespace VidShareWebApi.Data
{
    public class AppDbContext : DbContext
    {
        //getting di from the program.cs file and here i am telling to dbContext to use the same options
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<VideoInfo> VideoInfos { get; set; }
        public DbSet<VideoDownloadUrls> VideoDownloadUrls { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = 1,
                Email = "abc@gmail.com",
                Password = "xvasdfasf",
                CreatedAt = DateTime.UtcNow,
            });
        }


    }
}