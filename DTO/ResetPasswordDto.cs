using System;
using System.Collections.Generic;
using System.Text;

namespace Prunedge_User_Administration_Library.DTO
{
    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }
}
