using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace EComMVC.Models
{

    public class Authentication
    {

        private static string JwtKey = "C1CF4B7DC4C4175B6618DE4F55CA4";
        private static string JwtAudience = "SecureApiUser";
        private static string  JwtIssuer = "https://localhost:44381";
        private static Double JwtExpireDays = 30;

        public static string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username)

        };

    roles.ForEach(role => 
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(JwtExpireDays));

        var token = new JwtSecurityToken(
           JwtIssuer,
           JwtAudience,
            claims,
            expires: expires,
            signingCredentials: creds
        );

          return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string ValidateToken(string token)
{
    if (token == null)
        return null;

    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(JwtKey);
    try
    {
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var jti = jwtToken.Claims.First(claim => claim.Type == "jti").Value;
        var userName = jwtToken.Claims.First(sub => sub.Type == "sub").Value;

        return userName;
        }
        catch
            {
            return null;
            }
        }
    }
}
