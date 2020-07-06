using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Prunedge_User_Administration.Data.Entities;
using Prunedge_User_Administration.Data.JwtModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Security
{
    public class JwtSecurity : IJwtSecurity
    {
        private readonly IConfigurationSection _jwtConfig;

        public JwtSecurity(IConfiguration config)
        {
            _jwtConfig = config.GetSection("tokenManagement");


        }
        public JwtModel JwtGenerator( string userId)
        {
            var claim = new List<Claim>
           {
               new Claim(JwtRegisteredClaimNames.NameId, userId),
           };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.GetValue<string>("JwtKey")));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDecryptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(claims: claim),
                Expires = DateTime.Now.AddDays(_jwtConfig.GetValue<double>("JwtExpireDays")),
                SigningCredentials = credential,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDecryptor);
            var returnToken = tokenHandler.WriteToken(token);
            return new JwtModel
            {
                Token = returnToken,
                ExpiryDate = token.ValidTo
            };
        }
    }
}
