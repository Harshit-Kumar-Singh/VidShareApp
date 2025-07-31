using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using VidShareWebApi.DTOs;
using VidShareWebApi.Models;
using VidShareWebApi.UnitOfWork;
using VidShareWebApi.Utils;

namespace VidShareWebApi.Services.AuthService
{
    public class AuthService : IAuthService
    {

        private readonly PasswordHasher<User> passwordHasher = new  PasswordHasher<User>();
        private readonly IUnitOfWork unitOfWork;
        IConfiguration configuration;
        public AuthService(IUnitOfWork _unitOfWork, IConfiguration _config)
        {
            unitOfWork = _unitOfWork;
            configuration = _config;


        }
        public ServiceResult<string> LoginUser(LoginDto loginDto)
        {

            User user = unitOfWork.User.GetUser(loginDto.UserName);
            Console.WriteLine(user.Password +  " - "   + loginDto.Password);
            if (user == null)
            {
                return new ServiceResult<string>
                {
                    Message = "Either UserName or Password is Wrong",
                    Success = false,
                };
            }
            var userTemp = new User() { Email = user.Email };
            var verified = passwordHasher.VerifyHashedPassword(userTemp, user.Password, loginDto.Password);
            Console.WriteLine(verified.GetHashCode());
            bool result = verified == PasswordVerificationResult.Success ?  true : false;

            if (!result)
            {
                return new ServiceResult<string>
                {
                    Message = "Either UserName or Password is Wrong",
                    Success = false,
                };
            }


            var token = GenerateJSONWebToken(user);
            Console.WriteLine(token);

            return new ServiceResult<string>
            {
                Success = true,
                Message = "Login success",
                Result = token
            };
        }

        private string GenerateJSONWebToken(User user)
        {
            string jwtKey = configuration["JwtSettings:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myJwtKeymyJwtKeymyJwtKeymyJwtKey"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new[] {
                new Claim("UserId",user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName),
            };
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(1)
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        public async Task<ServiceResult<bool>> RegisterUser(RegisterDto registerDto)
        {
            try
            {

                var hmac = new HMACSHA512();
                var userTemp = new User() { Email = registerDto.Email };
                var user = new User
                {
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                    Password =  passwordHasher.HashPassword(userTemp,registerDto.Password)
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