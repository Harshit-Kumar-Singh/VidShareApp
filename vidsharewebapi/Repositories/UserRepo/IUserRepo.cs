using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories.Users
{
    public interface IUserRepo
    {
        Task<bool> SaveUser(User user);
        User GetUser(string userName);
    }
}