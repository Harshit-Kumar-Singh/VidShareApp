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
}