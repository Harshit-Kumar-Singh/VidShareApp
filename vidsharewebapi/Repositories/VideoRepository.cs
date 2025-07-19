using Amazon.DynamoDBv2.DataModel;
using VidShareWebApi.Models;
using VidShareWebApi.Repositories;
public class VideoRepository : IVideoRepository
{
    private readonly IDynamoDBContext _context;
    public VideoRepository(IDynamoDBContext context)
    {
        _context = context;
    }
    public async Task SaveAsync(UploadItem item)
    {
        await _context.SaveAsync(item);
    }
    public async Task UpdateAsync(UploadItem item)
    {
        var existingItem = await _context.LoadAsync<UploadItem>(item.Id);
        if (existingItem == null)
        {
            throw new Exception($"Item with {item.Id}");
        }
        existingItem.preSignedUrl = item.preSignedUrl ?? existingItem.preSignedUrl;
        existingItem.downloadRawVideoUrl = item.downloadRawVideoUrl ?? existingItem.downloadRawVideoUrl;
        existingItem.Status = item.Status ?? existingItem.Status;
        await _context.SaveAsync(existingItem);
        
    }
    public async Task<UploadItem> GetByIdAsync(string id)
    {
        return await _context.LoadAsync<UploadItem>(id);
    }
    
    
}