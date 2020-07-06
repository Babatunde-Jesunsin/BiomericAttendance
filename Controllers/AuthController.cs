using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Prunedge_User_Administration.Data.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Prunedge_User_Administration.Security;
using Prunedge_User_Administration.Data.Entities;
using Prunedge_User_Administration.Data.JwtModel;
using Prunedge_User_Administration.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Prunedge_User_Administration.Data;
using Prunedge_User_Administration_Library.DTO;

namespace Prunedge_User_Administration.Controllers
{
    
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private readonly IJwtSecurity _jwtSecurity;
        private readonly IAuthRepo _authRepo;
        private readonly ILogger<AuthController> _logger;

        public AuthController(


            IConfiguration configuration, IJwtSecurity jwtSecurity,
            IAuthRepo authRepo, ILogger<AuthController> logger
            )
        {


            _configuration = configuration;
            _jwtSecurity = jwtSecurity;
            _authRepo = authRepo;
            _logger = logger;
        }
       
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto model)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            JwtModel token = null;
            try
            {
                token = await _authRepo.Login(model);
                if (token == null) return BadRequest(new { Error = "User does not Exist or Invalid Password" });

            }
            catch (Exception e)
            {

                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            return Ok(token);
        }
        
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
            JwtModel token = null;
            try
            {
                var checkMail = await _authRepo.ExsitEmail(model.Email);
                if (checkMail) return BadRequest(new { Error = "Email already exist" });
                token = await _authRepo.Register(model);
                if (token == null) return BadRequest( "Password is not strong enough");

            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            return Ok(token);
        }
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);

            try
            {
                var id = _authRepo.GetCurrentUser();
                var user = await _authRepo.GetUserById(id);
                if (user == null) return NotFound(new { Error = "user not found" });
                bool isPasswordValid = await _authRepo.IsPasswordValid(model, user);
                if (!isPasswordValid) return BadRequest(new { Error = "incorrect password" });
                bool success = await _authRepo.ResetPassword(model, user);
                if (!success) return StatusCode(StatusCodes.Status500InternalServerError, "An Error Occured while resetting password");

            }
            catch (Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            return Ok(new { Succeded = true });

        }
        [HttpPut("UpdateProfile")]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {

            ApplicationUser result;

            try
            {
                var currentUserId = _authRepo.GetCurrentUser();
                var user = await _authRepo.GetUserById(currentUserId);
                if (user == null) return NotFound(new { Error = "User not found" });
                user.UserName = model.Email ?? user.UserName;
                user.Email = model.Email ?? user.Email;
                user.PhoneNumber = model.phoneNumber ?? user.PhoneNumber;
                result = await _authRepo.UpdateProfile(user);


            }
            catch (Exception e)
            {

                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
            return Ok(result);
        }
        [HttpPost("ResetPasswordLink")]
        public async Task<ActionResult> ResetPasswordLink( ResetPasswordDto model)
        {

            try
            {
                var result = await _authRepo.ResetUserPassword(model.Email);
                if (string.IsNullOrEmpty(result)) return StatusCode(StatusCodes.Status500InternalServerError, "Internal server Error");
            await  _authRepo.SendEmailAsync(model.Email, "reset password", result);
              return Ok("An email has been sent to your mail");
                
            }
            catch(Exception e)
            {
                _logger.LogError(e.InnerException?.ToString() ?? e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");

            }

        }
    }
}