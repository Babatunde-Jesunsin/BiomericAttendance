using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Prunedge_User_Administration.Data;
using Prunedge_User_Administration.Data.DTO;
using Prunedge_User_Administration.Data.Entities;

using Prunedge_User_Administration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Prunedge_User_Administration.Security.Model;
using Prunedge_User_Administration.Security;
using Prunedge_User_Administration.Data.JwtModel;
using System.Text;

namespace Prunedge_User_Administration.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtSecurity _jwtSecurity;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AdminstrationDbContext _dbContext;


        public AuthRepo(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IJwtSecurity jwtSecurity, IHttpContextAccessor httpContextAccessor, AdminstrationDbContext dbContext, IOptions<EmailAuthOptions> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSecurity = jwtSecurity;
            this.httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            Options = optionsAccessor.Value;
        }
        public EmailAuthOptions Options { get; }




        public async Task<bool> ExsitEmail(string inputEmail)
        {
            var email = await _userManager.FindByEmailAsync(inputEmail);
            if (email != null)
            {
                return true;
            }
            return false;
        }

        public async Task<JwtModel> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return null;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded)
            {

                var token = _jwtSecurity.JwtGenerator(user.Id);
                return token;
            }
            return null;

        }

        public async Task<JwtModel> Register(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var token = _jwtSecurity.JwtGenerator(user.Id);
                return token;
            }
            return null;

        }
        public async Task<bool> IsPasswordValid(ChangePasswordDto model, ApplicationUser user)
        {
            var checkOldPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (checkOldPassword)
            {
                return true;

            }
            return false;
        }


        public async Task<bool> ResetPassword(ChangePasswordDto model, ApplicationUser user)
        {

            //var token = await _userManager.GetSecurityStampAsync(user);
            //var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
            {

                return true;
            }
            // return false;

        }

        public string GetCurrentUser()
        {
            return httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(r => r.Type == ClaimTypes.NameIdentifier)?.Value;

        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);


        }


        public async Task<ApplicationUser> UpdateProfile(ApplicationUser user)
        {
            _dbContext.Update(user);
            if (await _dbContext.SaveChangesAsync() > 0) return user;
            return null;

        }
        //sendGrid
        public Task SendEmailAsync(string emails, string subject, string message)
        {

            return Execute(Environment.GetEnvironmentVariable("SENDEMAILDEMO_ENVIRONMENT_SENDGRID_KEY"), subject, message, emails);

            
        }

        public Task Execute(string apiKey, string subject, string message, string emails)
        {
            var messageWithLink = new StringBuilder();
            messageWithLink.Append($"<html><body>click on this link to reset your password <a href='{httpContextAccessor.HttpContext.Request.Host.Value + $"/api/auth/resetPasswordlink?email={emails}&link={message}"}></a> </body></html>");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@domain.com", "Bekenty Jean Baptiste"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };


            msg.AddTo(new EmailAddress(emails));


            Task response = client.SendEmailAsync(msg);
            return response;
        }
        public async Task<string> ResetUserPassword(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return string.Empty;
            var resetpasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            return resetpasswordToken;
            
        }

        public async Task<bool> ResetPasswordWithLink(string resetToken, string newPassword, ApplicationUser user)
        {

            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (result.Succeeded) return true;
            return false;
        }

        public async Task<ApplicationUser> UserByEmail(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User;
        }
    }
}

