using Prunedge_User_Administration.Data.Entities;
using Prunedge_User_Administration.Data.JwtModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Security
{
  public interface IJwtSecurity
    {
        JwtModel JwtGenerator(string userId);
    }
}
