namespace CardboardBox.Extensions.Scripting.Templating.Exceptions;

/// <summary>
/// Thrown when the definition of a required token is invalid
/// </summary>
/// <param name="token">The token that caused the exception</param>
/// <param name="reason">The reason the exception was thrown</param>
public class RequiredTokenInvalidException(
    string token, 
    string reason) 
    : Exception($"The given token is invalid: {reason} - \"{token}\"")
{
    /// <summary>
    /// The token that caused the exception
    /// </summary>
    public string Token { get; set; } = token;

    /// <summary>
    /// The reason the exception was thrown
    /// </summary>
    public string Reason { get; set; } = reason;
}
