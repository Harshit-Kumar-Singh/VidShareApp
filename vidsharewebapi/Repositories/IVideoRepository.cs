
using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories
{
    public interface IVideoRepository
    {
        Task SaveAsync(UploadItem item);
        Task<UploadItem> GetByIdAsync(string id);
        Task UpdateAsync(UploadItem item);
    }
}
