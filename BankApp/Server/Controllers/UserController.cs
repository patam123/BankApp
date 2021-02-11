using BankApp.Server.DataAccess;
using BankApp.Shared.Entities;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace BankApp.Server.Controllers
{
    [Route("api/user/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        Firestore firestore = new Firestore();

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

        [HttpPut]
        [Route("/user/update")]
        public async Task<string> Update([FromBody] User user)
        {
            var updateResult = await fireAuth.UpdateUser(user);
            if (updateResult == "Användare uppdaterad")
            {
                return await firestore.UpdateUser(user);
            }
            return updateResult;
        }

        [HttpDelete("{id}")]
        public async Task<string> Delete(string id)
        {
            Response.Cookies.Delete("session");
            var authResult = await fireAuth.DeleteUser(id);
            if (!string.IsNullOrEmpty(authResult))
            {
                return await firestore.DeleteUser(id);
            }
            return authResult;
        }

        [HttpGet("{id}")]
        public Task<User> GetUser(string id)
        {
            return firestore.GetUser(id);
        }

        [HttpPost]
        [Route("/user/createuserdoc")]
        public void CreateUser([FromBody] User user)
        {
            firestore.CreateUser(user);
        }

        [HttpPost]
        [Route("/user/cookie")]
        public async Task<ActionResult> LoginWithToken([FromBody] UserResponse request)
        {
            // Set session expiration to 5 days.
            var options = new SessionCookieOptions()
            {
                ExpiresIn = TimeSpan.FromDays(5),
            };

            try
            {
                // Create the session cookie. This will also verify the ID token in the process.
                // The session cookie will have the same claims as the ID token.
                var sessionCookie = await FirebaseAuth.DefaultInstance
                    .CreateSessionCookieAsync(request.IdToken, options);

                // Set cookie policy parameters as required.
                var cookieOptions = new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.Add(options.ExpiresIn),
                    HttpOnly = true,
                    Secure = true,
                };
                this.Response.Cookies.Append("session", sessionCookie, cookieOptions);
                return this.Ok();
            }
            catch (FirebaseAuthException)
            {
                return this.Unauthorized("Failed to create a session cookie");
            }
        }

        [HttpPost]
        [Route("/user/session")]
        public async Task<UserResponse> Profile()
        {
            var sessionCookie = this.Request.Cookies["session"];
            if (string.IsNullOrEmpty(sessionCookie))
            {
                var userResponse = new UserResponse();
                userResponse.ErrorMessage = "session cookie not available";
                return userResponse;
            }

            try
            {
                // Verify the session cookie. In this case an additional check is added to detect
                // if the user's Firebase session was revoked, user deleted/disabled, etc.
                var checkRevoked = true;
                var decodedToken = await FirebaseAuth.DefaultInstance.VerifySessionCookieAsync(
                    sessionCookie, checkRevoked);
                var userResponse = new UserResponse();
                userResponse.UserId = decodedToken.Uid;
                return userResponse;
            }
            catch (FirebaseAuthException)
            {
                // Session cookie is invalid or revoked. Force user to login.
                var userResponse = new UserResponse();
                userResponse.ErrorMessage = "session no longer valid";
                return userResponse;
            }
        }
        [HttpPost]
        [Route("/user/logout")]
        public void ClearSessionCookie()
        {
            Response.Cookies.Delete("session");
        }
    }
}
