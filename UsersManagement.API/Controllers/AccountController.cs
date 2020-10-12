
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersManagement.API.Interface;
using UsersManagement.Domain.Home;

namespace UsersManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IAuthenticateServices authenticationService;
        public AccountController(IAuthenticateServices authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> Login(User objUser)
        {
            var user = await authenticationService.Authenticate(objUser.USERID, objUser.PASSWORD);
            //Save token in session object
            //HttpContext.Session.SetString("JWToken", user.Token);
            return user;
        }
    }
}