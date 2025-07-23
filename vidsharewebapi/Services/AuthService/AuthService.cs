using System.Security.Cryptography;
using System.Text;
using VidShareWebApi.DTOs;
using VidShareWebApi.Models;
using VidShareWebApi.UnitOfWork;
using VidShareWebApi.Utils;

namespace VidShareWebApi.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        public AuthService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            
        }
        public Task<ServiceResult<bool>> LoginUser(LoginDto loginDto)
        {

            // Fix hash string implement login and create token logic
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> RegisterUser(RegisterDto registerDto)
        {
            try
            {
                
                var hmac = new HMACSHA512();
                var user = new User
                {
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                    Password = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password))),
                    PasswordSalt = Convert.ToBase64String(hmac.Key)
                };
                await unitOfWork.User.SaveUser(user);
                await unitOfWork.SaveChanges();
                unitOfWork.Dispose();
                return new ServiceResult<bool>
                {
                    Message = "User Registered Successfully!",
                    Success = true,
                    Result = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return new ServiceResult<bool>
                {
                    Message = "Registration Failed",
                    Success = false,
                    Result = false
                };
            }


        }
    }
}