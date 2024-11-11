using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace CardboardBox.Extensions.AspNetCore.Jwt;

/// <summary>
/// Extensions related to asp.net core objects
/// </summary>
public static class JwtExtensions
{
    /// <summary>
    /// The section in the configuration file for JWT settings
    /// </summary>
    public static string JwtConfigSection { get; set; } = "Jwt";

    /// <summary>
    /// The key for the JWT signing key
    /// </summary>
    public static string JwtConfigSectionKey { get; set; } = "Key";

    /// <summary>
    /// The key for the JWT audience
    /// </summary>
    public static string JwtConfigSectionAudience { get; set; } = "Audience";

    /// <summary>
    /// The key for the JWT issuer
    /// </summary>
    public static string JwtConfigSectionIssuer { get; set; } = "Issuer";

    /// <summary>
    /// Gets the value of a claim from a principal
    /// </summary>
    /// <param name="principal">The clients principal</param>
    /// <param name="claim">The claim to look for</param>
    /// <returns>The claim value</returns>
    public static string? Claim(this ClaimsPrincipal? principal, string claim)
    {
        return principal?.FindFirst(claim)?.Value;
    }

    /// <summary>
    /// Gets the symmetric security key from a string
    /// </summary>
    /// <param name="key">The security key</param>
    /// <returns>The symmetric security key</returns>
    public static SymmetricSecurityKey GetKey(this string key)
    {
        return new SymmetricSecurityKey(key.GetBytes());
    }

    /// <summary>
    /// Updates a claim in the current principal
    /// </summary>
    /// <param name="principal">The principal</param>
    /// <param name="key">The claims key</param>
    /// <param name="value">The claims value</param>
    public static void AddUpdateClaim(this IPrincipal principal, string key, string value)
    {
        if (principal.Identity is not ClaimsIdentity identity)
            return;

        // check for existing claim and remove it
        var existingClaim = identity.FindFirst(key);
        if (existingClaim != null)
            identity.RemoveClaim(existingClaim);

        // add new claim
        identity.AddClaim(new Claim(key, value));
    }

    /// <summary>
    /// Gets the claims value from the current principal
    /// </summary>
    /// <param name="principal">The principal</param>
    /// <param name="key">The key of the claim</param>
    /// <returns>The claim value</returns>
    public static string? GetClaimValue(this IPrincipal principal, string key)
    {
        if (principal.Identity is not ClaimsIdentity identity)
            return null;

        var claim = identity.Claims.FirstOrDefault(c => c.Type == key);
        return claim?.Value;
    }

    internal static string GetOAuth(this IConfiguration config, string key)
    {
        return config[JwtConfigSection + ":" + key]
            ?? throw new NullReferenceException($"{JwtConfigSection}:{key} is not set");
    }

    /// <summary>
    /// Gets the symmetric security key from the configuration
    /// </summary>
    /// <param name="config">The configuration</param>
    /// <returns>The JWT security key</returns>
    public static SymmetricSecurityKey GetKey(this IConfiguration config)
    {
        return config.GetOAuth(JwtConfigSectionKey).GetKey();
    }

    /// <summary>
    /// The audience of the token
    /// </summary>
    /// <param name="config">The configuration</param>
    /// <returns>The JWT audience</returns>
    public static string GetAudience(this IConfiguration config)
    {
        return config.GetOAuth(JwtConfigSectionAudience);
    }

    /// <summary>
    /// The issuer of the token
    /// </summary>
    /// <param name="config">The configuration</param>
    /// <returns>The JWT issuer</returns>
    public static string GetIssuer(this IConfiguration config)
    {
        return config.GetOAuth(JwtConfigSectionIssuer);
    }

    /// <summary>
    /// Get the token validation parameters from the configuration
    /// </summary>
    /// <param name="config">The configuration</param>
    /// <returns>The token validation parameters</returns>
    public static TokenValidationParameters GetParameters(this IConfiguration config)
    {
        return new TokenValidationParameters
        {
            IssuerSigningKey = config.GetKey(),
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = config.GetAudience(),
            ValidIssuer = config.GetIssuer(),
        };
    }

    /// <summary>
    /// Parsers the given JWT token using the parameters from the config
    /// </summary>
    /// <param name="config">The configuration</param>
    /// <param name="token">The token to parse</param>
    /// <returns>The token result</returns>
    public static JwtTokenResult ParseToken(this IConfiguration config, string token)
    {
        var validationParams = config.GetParameters();
        var handler = new JwtSecurityTokenHandler();
        var principals = handler.ValidateToken(token, validationParams, out var securityToken);
        return new(principals, securityToken);
    }
}
