using Microsoft.AspNetCore.SignalR;

namespace VidShareWebApi.Utils.SignalR
{
    public class UploadHub : Hub
    {
        public async Task JoinGroup(string uploadId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, uploadId);
        }
    }
}