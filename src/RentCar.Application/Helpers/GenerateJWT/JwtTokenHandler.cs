﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentCar.Application.Helpers.GenerateJWT;
using RentCar.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SecureLoginApp.Application.Helpers.GenerateJwt;

public class JwtTokenHandler : IJwtTokenHandler
{
    private readonly JwtOption _jwtOption;

    public JwtTokenHandler(IOptions<JwtOption> jwtOption)
    {
        _jwtOption = jwtOption.Value;
    }

    public string GenerateAccessToken(User user, string token)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("token", token) // optional

        };

        // Role va Permission'larni claimga qo‘shish
        if (user.UserRoles != null && user.UserRoles.Any())
        {
            foreach (var userRole in user.UserRoles)
            {
                // Role nomini qo‘shish (optional)
                if (!string.IsNullOrEmpty(userRole.Role?.Name))
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
                }

                // Permissionlarni qo‘shish
                if (userRole.Role?.RolePermissions != null)
                {
                    foreach (var rolePermission in userRole.Role.RolePermissions)
                    {
                        if (!string.IsNullOrEmpty(rolePermission.Permission?.ShortName))
                        {
                            claims.Add(new Claim("permission", rolePermission.Permission.ShortName));
                        }
                    }
                }
            }
        }

        // JWT token imzosi uchun kalit
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            expires: DateTime.UtcNow.AddSeconds(_jwtOption.ExpirationInSeconds),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}

