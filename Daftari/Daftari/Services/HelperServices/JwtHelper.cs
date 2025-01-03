﻿using Daftari.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Daftari.Services.HelperServices
{
    public class JwtHelper
    {
        public string GenerateToken(string userId, string userName, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.JWT.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userName),
            new Claim("UserId", userId),  // Add UserId as a custom claim
			new Claim(ClaimTypes.Role,role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
            issuer:Settings.JWT.Issuer,
            audience: Settings.JWT.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Settings.JWT.ExpireMinutes),
            signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }



    }
}
