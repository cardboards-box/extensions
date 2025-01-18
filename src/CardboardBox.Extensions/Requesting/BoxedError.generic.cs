namespace CardboardBox.Extensions.Requesting;

/// <summary>
/// The result of a failed API call
/// </summary>
/// <typeparam name="T">The type of data that should be present</typeparam>
public class BoxedError<T> : Boxed<T>
{
    /// <summary>
    /// The result of a failed API call
    /// </summary>
    /// <param name="code">The status code of the result</param>
    /// <param name="description">A brief description of the error</param>
    /// <param name="errors">Any issues that occurred</param>
    public BoxedError(HttpStatusCode code, string description, params string[] errors)
    {
        Code = (int)code;
        Description = description;
        Errors = errors;
        Type = ERROR;
    }

    /// <summary>
    /// The result of a failed API call (HTTP 500)
    /// </summary>
    /// <param name="description">A brief description of the error</param>
    /// <param name="errors">Any issues that occurred</param>
    public BoxedError(string description, params string[] errors)
        : this(HttpStatusCode.InternalServerError, description, errors) { }

    /// <summary>
    /// The result of a failed API call (HTTP 500)
    /// </summary>
    /// <param name="code">The status code of the result</param>
    /// <param name="description">A brief description of the error</param>
    /// <param name="errors">Any issues that occurred</param>
    public BoxedError(int code, string description, params string[] errors)
        : this((HttpStatusCode)code, description, errors) { }

    /// <summary>
    /// The result of a failed API call (HTTP 500)
    /// </summary>
    [JsonConstructor]
    public BoxedError() : this(HttpStatusCode.InternalServerError, "An error occurred") { }
}
