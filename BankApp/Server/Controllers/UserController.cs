using BankApp.Server.DataAccess;
using BankApp.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BankApp.Server.Controllers
{
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        FirebaseAuthentication fireAuth = new FirebaseAuthentication();

        [HttpPost]
        [Route("/user/register")]
        public Task<string> Register([FromBody] User user)
        {
            return fireAuth.CreateUser(user);
        }
        [HttpPost]
        [Route("/user/login")]
        public Task<UserResponse> LogIn([FromBody] User user)
        {
            return fireAuth.LogIn(user);
        }
    }
}
