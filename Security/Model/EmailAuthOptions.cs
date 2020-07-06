using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Security.Model
{
    public class EmailAuthOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
