using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CardboardBox.Extensions.AspNetCore.Jwt;

/// <summary>
/// Represents a JWT token
/// </summary>
/// <param name="key">The signing key</param>
public class JwtToken(SymmetricSecurityKey key)
{
    private List<Claim> _claims = [];

    /// <summary>
    /// Fetches a claim from the token
    /// </summary>
    /// <param name="key">The key of the claim</param>
    /// <returns>The value of the claim</returns>
    public string? this[string key]
    {
        get => _claims.Find(t => t.Type == key)?.Value;
        set
        {
            var claim = _claims.Find(t => t.Type == key);

            if (claim != null)
                _claims.Remove(claim);

            _claims.Add(new Claim(key, value ?? ""));
        }
    }

    /// <summary>
    /// The claim with the key <see cref="ClaimTypes.Email"/>
    /// </summary>
    public string? Email
    {
        get => this[ClaimTypes.Email];
        set => this[ClaimTypes.Email] = value;
    }

    /// <summary>
    /// The signing key
    /// </summary>
    public SymmetricSecurityKey Key { get; } = key;

    /// <summary>
    /// The issuer of the token
    /// </summary>
    public string Issuer { get; set; } = "";

    /// <summary>
    /// The audience of the token
    /// </summary>
    public string Audience { get; set; } = "";

    /// <summary>
    /// How many minutes the token is valid for
    /// </summary>
    public double ExpiryMinutes { get; set; } = 10080;

    /// <summary>
    /// The signing algorithm
    /// </summary>
    public string SigningAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;

    /// <summary>
    /// Represents a JWT token
    /// </summary>
    /// <param name="key">The signing key</param>
    /// <param name="token">The JWT token</param>
    public JwtToken(SymmetricSecurityKey key, string token) : this(key)
    {
        Read(token);
    }

    /// <summary>
    /// Represents a JWT token
    /// </summary>
    /// <param name="key">The signing key</param>
    public JwtToken(string key) : this(key.GetKey()) { }

    /// <summary>
    /// Represents a JWT token
    /// </summary>
    /// <param name="key">The signing key</param>
    /// <param name="token">The JWT token</param>
    public JwtToken(string key, string token) : this(key)
    {
        Read(token);
    }

    /// <summary>
    /// Add some claims to the token
    /// </summary>
    /// <param name="claims">The claims to add to the token</param>
    /// <returns>The JWT token for chaining</returns>
    public JwtToken AddClaim(params Claim[] claims)
    {
        foreach (var claim in claims)
            _claims.Add(claim);
        return this;
    }

    /// <summary>
    /// Add a claim to the token
    /// </summary>
    /// <param name="key">The key of the token</param>
    /// <param name="value">The value of the token</param>
    /// <returns>The JWT token for chaining</returns>
    public JwtToken AddClaim(string key, string value)
    {
        return AddClaim(new Claim(key, value));
    }

    /// <summary>
    /// Add some claims to the token
    /// </summary>
    /// <param name="claims">The claims to add to the token</param>
    /// <returns>The JWT token for chaining</returns>
    public JwtToken AddClaim(params (string, string)[] claims)
    {
        foreach (var claim in claims)
            AddClaim(claim.Item1, claim.Item2);

        return this;
    }

    /// <summary>
    /// Sets the expiry of the token
    /// </summary>
    /// <param name="minutes">The number of minutes</param>
    /// <returns>The JWT token for chaining</returns>
    public JwtToken Expires(double minutes)
    {
        ExpiryMinutes = minutes;
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns>The JWT token for chaining</returns>
    public JwtToken SetEmail(string email)
    {
        Email = email;
        return this;
    }

    /// <summary>
    /// Sets the issuer of the token
    /// </summary>
    /// <param name="issuer">The issuer of the token</param>
    /// <returns>The JWT token for chaining</returns>
    public JwtToken SetIssuer(string issuer)
    {
        Issuer = issuer;
        return this;
    }

    /// <summary>
    /// Sets the audience of the token
    /// </summary>
    /// <param name="audience">The audience of the token</param>
    /// <returns>The JWT token for chaining</returns>
    public JwtToken SetAudience(string audience)
    {
        Audience = audience;
        return this;
    }

    /// <summary>
    /// Writes the JWT token to a string
    /// </summary>
    /// <returns>The JWT token as a string</returns>
    public string Write()
    {
        this[JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString();

        var token = new JwtSecurityToken
        (
            issuer: Issuer,
            audience: Audience,
            claims: _claims,
            expires: DateTime.UtcNow.AddMinutes(ExpiryMinutes),
            signingCredentials: new SigningCredentials(Key, SigningAlgorithm)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private void Read(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var validations = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = Key,
            ValidateIssuerSigningKey = true
        };

        _claims = handler.ValidateToken(token, validations, out SecurityToken ts).Claims.ToList();

        var t = (JwtSecurityToken)ts;
        Issuer = t.Issuer;
        Audience = t.Audiences.First();
        ExpiryMinutes = (t.ValidTo - DateTime.Now).TotalMinutes;
        SigningAlgorithm = t.SignatureAlgorithm;
    }
}
