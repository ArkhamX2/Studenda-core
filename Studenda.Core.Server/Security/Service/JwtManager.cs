﻿using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Studenda.Core.Server.Security.Service;

public static class JwtManager
{
    /// <summary>
    ///     издатель токена
    /// </summary>
    public const string Issuer = "MyAuthServer";

    /// <summary>
    ///     потребитель токена
    /// </summary>
    public const string Audience = "MyAuthClient";

    /// <summary>
    ///     ключ
    /// </summary>
    private const string Key = "mysupersecret_secretkey!123";

    public static IEnumerable<Claim> CreateClaims(this IdentityUser identityUser, IEnumerable<IdentityRole> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, identityUser.Id),
            new(ClaimTypes.Role, string.Join(" ", roles.Select(role => role.Name)))
        };

        if (identityUser.Email is not null)
        {
            claims.Add(new Claim(ClaimTypes.Email, identityUser.Email));
        }

        return claims;
    }

    public static JwtSecurityToken CreateJwtToken(this IEnumerable<Claim> claims, IConfiguration configuration)
    {
        var expire = configuration.GetSection("Jwt:Expire").Get<int>();
        var key = GetSymmetricSecurityKey();

        return new JwtSecurityToken(
            Issuer,
            Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expire),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
    }

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}