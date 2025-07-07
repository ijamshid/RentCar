using Microsoft.Extensions.Options;
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
        var claims = new List<Claim>()
    {
        new Claim(CustomClaimNames.Id, user.Id.ToString()),
        new Claim(CustomClaimNames.Email, user.Email),
        new Claim(CustomClaimNames.Token, token)
    };

        if (user.UserRoles != null && user.UserRoles.Any())
        {
            foreach (var userRole in user.UserRoles)
            {
                if (userRole.Role?.RolePermissions != null)
                {
                    foreach (var rolePermission in userRole.Role.RolePermissions)
                    {
                        if (rolePermission.Permission != null && !string.IsNullOrEmpty(rolePermission.Permission.ShortName))
                        {
                            claims.Add(new Claim("permission", rolePermission.Permission.ShortName));
                        }
                    }
                }
            }
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.SecretKey));

        var jwtToken = new JwtSecurityToken(
            issuer: _jwtOption.Issuer,
            audience: _jwtOption.Audience,
            expires: DateTime.Now.AddSeconds(_jwtOption.ExpirationInSeconds),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }


    public string GenerateRefreshToken()
    {
        byte[] bytes = new byte[64];

        using var randomGenerator =
            RandomNumberGenerator.Create();

        randomGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
