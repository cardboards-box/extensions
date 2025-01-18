namespace CardboardBox.Extensions.Requesting;

/// <summary>
/// Represents the base return result for all API calls
/// </summary>
public class Boxed
{
    /// <summary>
    /// Result was successful and contained no data
    /// </summary>
    public const string OK = "ok";

    /// <summary>
    /// Result was an error
    /// </summary>
    public const string ERROR = "error";

    /// <summary>
    /// Result was successful and contained data
    /// </summary>
    public const string DATA = "data";

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    public const string ARRAY = "array";

    /// <summary>
    /// Result was successful and contained a paged collection of data
    /// </summary>
    public const string PAGED = "paged";

    /// <summary>
    /// Request ID (useful for debugging)
    /// </summary>
    [JsonPropertyName("requestId")]
    public Guid RequestId { get; set; }

    /// <summary>
    /// The boxed code for the result
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = OK;

    /// <summary>
    /// The HTTP status code for the result
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// A brief description of the error
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Any issues that occurred
    /// </summary>
    [JsonPropertyName("errors")]
    public string[]? Errors { get; set; }

    /// <summary>
    /// Whether or not the result was successful
    /// </summary>
    [JsonIgnore]
    public bool Success => Code >= 200 && Code < 300;

    /// <summary>
    /// The number of milliseconds the request took to complete
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Elapsed { get; set; }

    /// <summary>
    /// Represents the base return result for all API calls
    /// </summary>
    [JsonConstructor]
    public Boxed() { }

    /// <summary>
    /// Represents the base return result for all API calls
    /// </summary>
    /// <param name="code">The HTTP status code result</param>
    /// <param name="type">The type of result</param>
    public Boxed(int code, string? type = null)
    {
        Code = code;

        if (!string.IsNullOrWhiteSpace(type))
        {
            Type = type;
            return;
        }

        Type = code >= 200 && code < 300 ? OK : ERROR;
    }

    /// <summary>
    /// Represents the base return result for all API calls
    /// </summary>
    /// <param name="code">The HTTP status code</param>
    /// <param name="type">The type of result</param>
    public Boxed(HttpStatusCode code, string? type = null) : this((int)code, type) { }

    /// <summary>
    /// Result was successful and contained no data
    /// </summary>
    /// <returns>The returned result</returns>
    public static Boxed Ok() => new(HttpStatusCode.OK);

    /// <summary>
    /// Result was successful and contained data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static Boxed<T> Ok<T>(T data) => new(data);

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedArray<T> Ok<T>(params T[] data) => new(data);

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedArray<T> Ok<T>(IEnumerable<T> data) => new(data.ToArray());

    /// <summary>
    /// Result was successful and contained a collection of data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedArray<T> Ok<T>(List<T> data) => new([.. data]);

    /// <summary>
    /// Result was successful and contained a collection of paged data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="pages">The number of pages in the result</param>
    /// <param name="total">The total number of results</param>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedPaged<T> Ok<T>(int pages, int total, params T[] data)
    {
        return new BoxedPaged<T>(data, pages, total);
    }

    /// <summary>
    /// Result was successful and contained a collection of paged data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="pages">The number of pages in the result</param>
    /// <param name="total">The total number of results</param>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedPaged<T> Ok<T>(int pages, int total, IEnumerable<T> data)
    {
        return new BoxedPaged<T>(data.ToArray(), pages, total);
    }

    /// <summary>
    /// Result was successful and contained a collection of paged data
    /// </summary>
    /// <typeparam name="T">The type of data</typeparam>
    /// <param name="pages">The number of pages in the result</param>
    /// <param name="total">The total number of results</param>
    /// <param name="data">The result data</param>
    /// <returns>The returned result</returns>
    public static BoxedPaged<T> Ok<T>(int pages, int total, List<T> data)
    {
        return new BoxedPaged<T>([.. data], pages, total);
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <param name="errors">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static BoxedError Exception(params string[] errors)
    {
        return new BoxedError("500 - An error occurred", errors);
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <param name="exceptions">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static BoxedError Exception(params Exception[] exceptions)
    {
        return new BoxedError("500 - An error occurred", exceptions.Select(e => e.Message).ToArray());
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="errors">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static BoxedError<T> Exception<T>(params string[] errors)
    {
        return new BoxedError<T>("500 - An error occurred", errors);
    }

    /// <summary>
    /// An exception occurred
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="exceptions">The error(s) that occurred</param>
    /// <returns>The returned error result</returns>
    public static BoxedError<T> Exception<T>(params Exception[] exceptions)
    {
        return new BoxedError<T>("500 - An error occurred", exceptions.Select(e => e.Message).ToArray());
    }

    /// <summary>
    /// Something is missing
    /// </summary>
    /// <param name="resource">The resource that was missing</param>
    /// <param name="errors">Any other issues that occurred</param>
    /// <returns>The returned error result</returns>
    public static BoxedError NotFound(string resource, params string[] errors)
    {
        return new BoxedError(HttpStatusCode.NotFound, "404 - Something is missing", [$"The requested resource '{resource}' was not found", .. errors]);
    }

    /// <summary>
    /// Something is missing
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="resource">The resource that was missing</param>
    /// <param name="errors">Any other issues that occurred</param>
    /// <returns>The returned error result</returns>
    public static BoxedError<T> NotFound<T>(string resource, params string[] errors)
    {
        return new BoxedError<T>(HttpStatusCode.NotFound, "404 - Something is missing", [$"The requested resource '{resource}' was not found", .. errors]);
    }

    /// <summary>
    /// User is unauthorized
    /// </summary>
    /// <param name="issues">Any issues that occurred that caused the unauthorized error</param>
    /// <returns>The returned error result</returns>
    public static BoxedError Unauthorized(params string[] issues)
    {
        return new BoxedError(HttpStatusCode.Unauthorized, "401 - Unauthorized", ["You are not authorized to access this resource", .. issues]);
    }

    /// <summary>
    /// User is unauthorized
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="issues">Any issues that occurred that caused the unauthorized error</param>
    /// <returns>The returned error result</returns>
    public static BoxedError<T> Unauthorized<T>(params string[] issues)
    {
        return new BoxedError<T>(HttpStatusCode.Unauthorized, "401 - Unauthorized", ["You are not authorized to access this resource", .. issues]);
    }

    /// <summary>
    /// Something the user did was bad
    /// </summary>
    /// <param name="errors">The different errors</param>
    /// <returns>The returned error result</returns>
    public static BoxedError Bad(params string[] errors)
    {
        return new BoxedError(HttpStatusCode.BadRequest, "400 - User input is bad", errors);
    }

    /// <summary>
    /// Something the user did was bad
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="errors">The different errors</param>
    /// <returns>The returned error result</returns>
    public static BoxedError<T> Bad<T>(params string[] errors)
    {
        return new BoxedError<T>(HttpStatusCode.BadRequest, "400 - User input is bad", errors);
    }

    /// <summary>
    /// Something the user did was bad
    /// </summary>
    /// <param name="errors">The different errors</param>
    /// <returns>The returned error result</returns>
    public static BoxedError Bad(IEnumerable<string> errors) => Bad(errors.ToArray());

    /// <summary>
    /// Something the user did was bad
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="errors">The different errors</param>
    /// <returns>The returned error result</returns>
    public static BoxedError<T> Bad<T>(IEnumerable<string> errors) => Bad<T>(errors.ToArray());

    /// <summary>
    /// Resource already exists
    /// </summary>
    /// <param name="resource">The resource that was conflicting</param>
    /// <returns>The result of the request</returns>
    public static BoxedError Conflict(string resource)
    {
        return new BoxedError(HttpStatusCode.Conflict, "409 - Already exists", $"The resource '{resource}' already exists");
    }

    /// <summary>
    /// Resource already exists
    /// </summary>
    /// <typeparam name="T">The type of data that should be present</typeparam>
    /// <param name="resource">The resource that was conflicting</param>
    /// <returns>The result of the request</returns>
    public static BoxedError<T> Conflict<T>(string resource)
    {
        return new BoxedError<T>(HttpStatusCode.Conflict, "409 - Already exists", $"The resource '{resource}' already exists");
    }
}
