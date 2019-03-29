namespace Notes.API.Controllers
{
    using System.Threading;
    using Microsoft.AspNetCore.Mvc;
    using Client.Users;
    using API.Auth;
    using Notes.API.Errors;

    [Route("v1/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthenticator authenticator;

        public AuthController(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        [Route("")]
        public IActionResult Auth([FromBody] UserAuthenticationInfo authenticationInfo)
        {
            if (!this.ModelState.IsValid)
            {
                var error = ServiceErrorResponses.BodyIsMissing(nameof(UserAuthenticationInfo));
                return this.BadRequest();
            }
            SessionState sessionState;
            try
            {
                sessionState = this.authenticator.AuthenticateAsync(authenticationInfo.Login, 
                    authenticationInfo.Password, new CancellationToken()).Result;
            }
            catch(AuthenticationException)
            {
                return this.Unauthorized();
            }

            this.HttpContext.Response.Cookies.Append("user_id", sessionState.UserId.ToString());
            this.HttpContext.Response.Cookies.Append("pass_hash", sessionState.PasswordHash);
            this.HttpContext.Response.Cookies.Append("session_id", sessionState.SessionId);
            return this.Ok();
        }
    }
}
