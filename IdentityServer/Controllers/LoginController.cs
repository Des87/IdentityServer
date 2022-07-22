using IdentityServer.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginManager loginManager;

        public LoginController(ILoginManager loginManager)
        {
            this.loginManager = loginManager;
        }

        [HttpGet]
        [Route("{email}/{password}")]
        [Produces("application/json")]
        public async Task<IActionResult> ConfirmEmail(string email, string password)
        {
            try
            {
                var token = loginManager.GetToken(email, password);
                return StatusCode(200, token);
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (SecurityTokenInvalidLifetimeException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }
        }

    }
}
