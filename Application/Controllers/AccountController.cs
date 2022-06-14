using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contract.Account;

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


      
    }
}
