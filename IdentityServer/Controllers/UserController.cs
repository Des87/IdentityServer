using IdentityServer.Helper;
using IdentityServer.Manager;
using IdentityServer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPut]
        [Route("")]
        [Produces("application/json")]
        public async Task<IActionResult> Create(string username = "Des", string password ="valami", string email="istvan.krasnyanszki@gmail.com", string lastname="Krasnyánszki", string fisrtname="István", string phonenumber = "0630-1132867")
        {
            try
            {
                userManager.CreateUser(username, password, email, lastname, fisrtname, phonenumber);
                return StatusCode(201, "Ok");
            }
            catch (InvalidDataException ex)
            {
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
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
                userManager.GetConfirmEmail(AuthToken);
                return StatusCode(200, "Confirmed");
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
                return StatusCode(204, "Email Sended");
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
        [HttpPost]
        [Route("ResetPassword/{email}/{userName}")]
        [Produces("application/json")]
        public async Task<IActionResult> ResetPassword(string email, string userName)
        {
            try
            {
                userManager.GetResetPasswordEmail(email, userName);
                return StatusCode(200, "Email Sended");
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
        [HttpGet]
        [Route("GetUser")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUser()
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
                var user = userManager.GetUser(email);
                return StatusCode(200, user);
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
        [HttpPost]
        [Route("ChangePassword")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
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
                userManager.ChangePassword(email, newPassword);
                return StatusCode(201, "Password Changed");
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
        [HttpPost]
        [Route("ChangePhoneNumber")]
        [Produces("application/json")]
        public async Task<IActionResult> ChangePhoneNumber(string newPhoneNumber)
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
                userManager.ChangePhonaNumber(email, newPhoneNumber);
                return StatusCode(201, "PhoneNumber Changed");
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
