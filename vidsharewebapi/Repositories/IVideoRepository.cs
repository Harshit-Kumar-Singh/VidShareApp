
using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories
{
    public interface IVideoRepository
    {
        Task SaveAsync(UploadItem item);
    }
}
