using Common.Exceptions;
using Entities.Owin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contract.Account;
using Services.DTO.Account;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using WebFramework.Filters;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiResultFilter]
    public class AccountController : ControllerBase
    { 
        public IAccountService _service { get; }
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        #region [-Login-]
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<string>> Login(string username, string password, bool rememberMe)
        {

            var jwt = await _service.CheckUserAndPassword(username, password, rememberMe);
            if (jwt != null)
            {
                return jwt;
            }
            return BadRequest("نام کاربری یا رمز عبور اشتباه است");


        }
        #endregion
        #region [-Register-]

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<Microsoft.AspNetCore.Identity.IdentityResult>> Create(UserDTO userDto)
        {

            var exists = await _service.UserNameExistAysnc(userDto.UserName);
            if (exists)
                return BadRequest("نام کاربری تکراری است");

            var user = Convert_UserDTO_TO_User(userDto);


            var result = await _service.Register(user, userDto.Password);

            return result;
        } 
        #endregion
        #region [UserManager-]

        [HttpPut("[action]")]
        public async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await _service.GetUser(id,cancellationToken);
            if (updateUser==null)
                return NotFound();


            await _service.UpdateUser(user, cancellationToken);

            return Ok();
        }
        [HttpDelete("[action]")]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var User = await _service.GetUser(id, cancellationToken);
            if (User == null)
                return NotFound();


            await _service.DeleteUser(User, cancellationToken);

            return Ok();
        }
        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var User = await _service.GetUser(id, cancellationToken);
            if (User == null)
                return NotFound();
            return User;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
   
            var users = await _service.GetUsers(cancellationToken);
            return Ok(users);
        }
        #endregion
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
