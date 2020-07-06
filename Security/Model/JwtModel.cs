using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Data.JwtModel
{
    public class JwtModel
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
