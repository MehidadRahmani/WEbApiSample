using Common.Exceptions;
using Entities.Owin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contract.Account;
using Services.DTO.Account;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    { 
        public IAccountService _service { get; }
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<string> Login(string username, string password,bool rememberMe)
        {
           
            var jwt = await _service.CheckUserAndPassword(username,password,rememberMe);
            if (jwt !=null)
            {
                return jwt;
            }
          
            return ("نام کاربری یا رمز عبور اشتباه است");
        
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResult<Microsoft.AspNetCore.Identity.IdentityResult>> Create(UserDTO userDto)
        {

            var exists = await _service.UserNameExistAysnc(userDto.UserName);
            if (exists)
               return BadRequest("نام کاربری تکراری است");

          var user = Convert_UserDTO_TO_User(userDto);


            var result = await _service.Register(user,userDto.Password);

            return result;
        }


        #region [-Internal Methods-]
        private User Convert_UserDTO_TO_User(UserDTO userDto)
        {
            var user = new User
            {
                Age = userDto.Age,
                FullName = userDto.FullName,
                Gender = userDto.Gender,
                UserName = userDto.UserName,
                Email = userDto.Email
            };
            return user;
        } 
        #endregion
    }
}
