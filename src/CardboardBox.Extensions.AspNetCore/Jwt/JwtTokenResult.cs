using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CardboardBox.Extensions.AspNetCore.Jwt;

/// <summary>
/// The result of a JWT token parse
/// </summary>
/// <param name="Principal">The parsed claims principal</param>
/// <param name="Token">The security token</param>
public record class JwtTokenResult(ClaimsPrincipal Principal, SecurityToken Token);
