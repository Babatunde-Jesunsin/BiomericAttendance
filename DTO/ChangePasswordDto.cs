using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Data.DTO
{
    public class ChangePasswordDto
    {
      

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }

    }
}
