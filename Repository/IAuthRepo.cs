using Prunedge_User_Administration.Data;
using Prunedge_User_Administration.Data.DTO;
using Prunedge_User_Administration.Data.Entities;
using Prunedge_User_Administration.Data.JwtModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Repository
{
    public interface IAuthRepo
    {
        Task<JwtModel> Register(RegisterDto model);
        Task<bool> ExsitEmail(string inputEmail);
        Task<JwtModel> Login(LoginDto model);
        Task<bool> ResetPassword(ChangePasswordDto model, ApplicationUser user);
        Task<bool> IsPasswordValid(ChangePasswordDto model, ApplicationUser user);
        string GetCurrentUser();
        Task<ApplicationUser> GetUserById(string id);
        Task<ApplicationUser> UpdateProfile( ApplicationUser user);
        Task SendEmailAsync(string emails, string subject, string message);
        Task<string> ResetUserPassword(string userEmail);
        Task<bool> ResetPasswordWithLink(string resetToken, string newPassword, ApplicationUser user);
        Task<ApplicationUser> UserByEmail(string email);



    }
}
