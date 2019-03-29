namespace Notes.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using API.Register;
    using Client.Users;
    using API.Errors;

    [Route("v1/register")]
    public class RegisterController : Controller
    {
        private readonly IRegistrator registrator;
        public RegisterController(IRegistrator registrator)
        {
            this.registrator = registrator;
        }

        [Route("")]
        public IActionResult Register([FromBody] UserRegistrationInfo userRegistrationInfo)
        {
            if (!this.ModelState.IsValid)
            {
                var error = ServiceErrorResponses.BodyIsMissing(nameof(UserRegistrationInfo));
                return this.BadRequest(error);
            }
            RegisterResult result = null;
            try
            {
                result = this.registrator.Register(userRegistrationInfo.Login, userRegistrationInfo.Password);
            }
            catch (RegistrationException ex)
            {
                return this.BadRequest(ex.Message);
            }
            return this.Ok(result);
        }
    }
}
