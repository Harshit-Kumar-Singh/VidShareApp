using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories.VideoInfoRepo
{
    public interface IVideoInfoRepo
    {
        Task<bool> SaveInfo(VideoInfo videoInfo);
    }
}