using VidShareWebApi.DTOs;
using VidShareWebApi.Utils;

namespace VidShareWebApi.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResult<bool>> RegisterUser(RegisterDto registerDto);
        ServiceResult<bool> LoginUser(LoginDto loginDto);
    }
}