using IdentityServer.DTOs;
using IdentityServer.Helper;
using IdentityServer.Manager;
using IdentityServer.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPut]
        [Route("CreateUser")]
        [Produces("application/json")]
        public async Task<IActionResult> Create([FromBody] RegistrationDTO registration)
        {
            try
            {
                userManager.CreateUser(registration);
                return await Task.FromResult(StatusCode(201, "Ok"));
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpGet]
        [Route("ConfirmEmail/{AuthToken}")]
        [Produces("application/json")]
        public async Task<IActionResult> ConfirmEmail(string AuthToken)
        {
            try
            {
                string email = "";
                if (!string.IsNullOrEmpty(AuthToken))
                {
                    email = TokenHelper.GetUserFromToken(AuthToken);
                    if (email == null) { throw new UnauthorizedAccessException("Unauthorized"); }
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized");
                }
                userManager.GetConfirmEmail(email);
                return await Task.FromResult(StatusCode(200, "Confirmed"));
            }
            catch (SecurityTokenInvalidLifetimeException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpPost]
        [Route("GetConfirmEmail")]
        [Produces("application/json")]
        public async Task<IActionResult> GetConfirmEmail()
        {
            try
            {
                var tokenstring = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
                string email = "";
                if (!string.IsNullOrEmpty(tokenstring))
                {
                    email = TokenHelper.GetUserFromToken(tokenstring);
                    if (email == null) { throw new UnauthorizedAccessException("Unauthorized"); }
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized");
                }
                userManager.GetConfirmEmail(email);
                return await Task.FromResult(StatusCode(204, "Email Sended"));
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
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpGet]
        [Route("Exist/{email}/{userName}")]
        [Produces("application/json")]
        public async Task<IActionResult> IsExist(string email, string userName)
        {
            try
            {
                var isExist = userManager.IsUserExist(email, userName);
                if (isExist == "NotValidEmail")
                {
                    return await Task.FromResult(StatusCode(200, "NotValidEmail"));

                }
                else if (isExist == "Email UserName")
                {
                    return await Task.FromResult(StatusCode(200, "Username and Email exist"));
                }
                else if (isExist == "Email ")
                {
                    return await Task.FromResult(StatusCode(200, "Email exist"));
                }
                else if (isExist == "UserName")
                {
                    return await Task.FromResult(StatusCode(200, "Username exist"));
                }
                else
                {
                    return await Task.FromResult(StatusCode(200, "doesn't exist"));
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpPost]
        [Route("ResetPassword/{email}/{userName}")]
        [Produces("application/json")]
        public async Task<IActionResult> ResetPassword(string email, string userName)
        {
            try
            {
                userManager.GetResetPasswordEmail(email, userName);
                return await Task.FromResult(StatusCode(200, "Email Sended"));
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(401, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpGet]
        [Route("GetUser")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUser(/*bool getAll = false*/)
        {
            try
            {
                var tokenstring = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
                string email = "";
                if (!string.IsNullOrEmpty(tokenstring))
                {
                    email = TokenHelper.GetUserFromToken(tokenstring);
                    if (email == null) { throw new UnauthorizedAccessException("Unauthorized"); }
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized");
                }
                var user = userManager.GetUser(email/*, getAll*/);
                return await Task.FromResult(StatusCode(200, user));
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
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
        [HttpPut]
        [Route("ChangeDetails")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangeDetails([FromBody] ChangesDTO? changes)
        {
            try
            {
                var tokenstring = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
                string email = "";
                if (!string.IsNullOrEmpty(tokenstring))
                {
                    email = TokenHelper.GetUserFromToken(tokenstring);
                    if (email == null) { throw new UnauthorizedAccessException("Unauthorized"); }
                }
                else
                {
                    throw new UnauthorizedAccessException("Unauthorized");
                }
                userManager.ChangeDetails(email, changes);
                return await Task.FromResult(StatusCode(201, "Details Changed"));
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
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
