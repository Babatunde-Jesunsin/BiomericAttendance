using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime DateJoined { get; set; }
    }
    
}
