using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.DTOs;
using VidShareWebApi.Services.AuthService;
using VidShareWebApi.Utils;

namespace VidShareWebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService _authService)
        {
            this.authService = _authService;
        }


        [HttpPost]
        [Route("register-user")]
        public async Task<ServiceResult<bool>> RegisterUser([FromBody] RegisterDto registerDto)
        {
            return await authService.RegisterUser(registerDto);
        }

        [HttpPost]
        [Route("login-user")]
        public ServiceResult<string> LoginUser([FromBody] LoginDto loginDto)
        {
            Console.WriteLine("hit");
            return authService.LoginUser(loginDto);
        }
    }
}