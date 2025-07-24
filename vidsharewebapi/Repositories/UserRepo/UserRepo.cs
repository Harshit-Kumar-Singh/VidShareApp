using VidShareWebApi.Data;
using VidShareWebApi.Models;

namespace VidShareWebApi.Repositories.Users
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext context;
        public UserRepo(AppDbContext _context)
        {
            context = _context;
        }

        public async Task<bool> SaveUser(User user)
        {
            try
            {
                await context.Users.AddAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;

            }

            throw new NotImplementedException();
        }


        public User GetUser(string userName)
        {
           return context.Users.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
        }
    }
}